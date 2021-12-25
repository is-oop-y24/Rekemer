
namespace LAB6
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;


    namespace ClientApplication
    {
        class Program
        {
            static void Main(string[] args)
            {
                Socket m_ListenSockeet;
                m_ListenSockeet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int iPort = 2084; // Set port number
                IPEndPoint m_LocalIPEndPoint = new IPEndPoint(IPAddress.Any, iPort);
                m_ListenSockeet.Bind(m_LocalIPEndPoint);
                m_ListenSockeet.Listen(4);


                // Create the socket
                //Input IP address and port
                string srvrIP = "";
                string srvrPort = "";
                Console.Write("Enter Server IP Address - dotted quad notation - : ");
                srvrIP = Console.ReadLine();
                Console.Write("Enter Server Port Number : ");
                srvrPort = Console.ReadLine();
                Console.WriteLine("\nSending to : " + srvrIP + " : " + srvrPort);

                // Create the socket based on date input
                Socket m_sendSocket;
                m_sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                //Connect to the endpoint (Server)
                // convert string IP and Port into IPAddress and int data types
                IPAddress destinationIP = IPAddress.Parse(srvrIP);
                int destinationPort = System.Convert.ToInt16(srvrPort);
                IPEndPoint destinationEP = new IPEndPoint(destinationIP, destinationPort);

                // User Message
                Console.WriteLine("\nWaiting to Connect... ");


                m_sendSocket.Connect(destinationEP);
                Socket m_AcceptedSocket = m_ListenSockeet.Accept();
                // User Message
                Console.WriteLine("Connected... ");


                //Send Information
                // Input data from user
                string msg = "";
                byte[] ReceiveBuffer = new byte[1024];
                int iReceiveByteCount;
                string response = "";

                iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
                response = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                Console.WriteLine(response);

                while (msg != "quit")
                {
                    Console.Write("\nEnter Message to Send : ");
                    msg = Console.ReadLine();
                    byte[] b_Data = System.Text.Encoding.ASCII.GetBytes(msg);
                    // User Message
                    Console.WriteLine("\nSending Data... ");

                    m_sendSocket.Send(b_Data, SocketFlags.None);
                    // User Message
                    Console.WriteLine("Sending Complete... " + iReceiveByteCount);
                    Console.WriteLine("\nGetting Data... ");
                    iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
                    response = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                    Console.WriteLine(response);
                    Console.WriteLine("\n Data Got ");
                }
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
}