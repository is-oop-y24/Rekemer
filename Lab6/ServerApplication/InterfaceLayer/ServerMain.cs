using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApplication.InterfaceLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create Socket
            Socket m_ListenSockeet;
            m_ListenSockeet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind the socket
            int iPort = 2085; // Set port number
            IPEndPoint m_LocalIPEndPoint = new IPEndPoint(IPAddress.Any, iPort);
            m_ListenSockeet.Bind(m_LocalIPEndPoint); // Bind

            // Optional code to display IP Address and Port Number
            Console.WriteLine(" Server IP Address : " + LocalIPAddress());
            Console.WriteLine(" Listening on Port : " + iPort);

            // Start Listening
            m_ListenSockeet.Listen(4);

            // Accept incoming connection
            Socket m_AcceptedSocket = m_ListenSockeet.Accept();

            // Start Receiving (Can send too)
            byte[] ReceiveBuffer = new byte[1024];
            int iReceiveByteCount;

            iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
            string msg = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
            Console.WriteLine(msg);


            while (msg != "quit")
            {
                iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);

                if (iReceiveByteCount > 0)
                {
                    //Display the message
                    msg = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                    Console.WriteLine(msg);
                }
            }

            m_AcceptedSocket.Shutdown(SocketShutdown.Both);


            m_AcceptedSocket.Close();
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName()); //gets the name of the current host
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //Finds the IP which maches internetwork
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            return localIP;
        }
    }
}