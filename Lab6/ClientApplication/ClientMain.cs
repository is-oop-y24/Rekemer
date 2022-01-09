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
            static Socket _ListenSockeet;
            private static Socket _sendSocket;

            static void Main(string[] args)
            {
                _ListenSockeet = InitListenSocket();
                // Create the socket
                //Input IP address and port
                string srvrIP = "";
                string srvrPort = "";
                Console.Write("Enter Server IP Address - dotted quad notation - : ");
                srvrIP = Console.ReadLine();
                Console.Write("Enter Server Port Number : ");
                srvrPort = Console.ReadLine();
                Console.WriteLine("\nSending to : " + srvrIP + " : " + srvrPort);
                _sendSocket = MSendSocket(srvrIP, srvrPort);
                Socket m_AcceptedSocket = _ListenSockeet.Accept();
                Console.WriteLine("Connected... ");

                string msg = "";
                byte[] ReceiveBuffer = new byte[1024];
                int iReceiveByteCount;
                string response = "";
                iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
                response = ParseBytes(ReceiveBuffer, iReceiveByteCount);
                Console.WriteLine(response);

                while (msg != "quit")
                {
                    Console.Write("\nEnter Message to Send : ");
                    msg = Console.ReadLine();
                    byte[] b_Data = TransformToBytes(msg);
                    // User Message
                    Console.WriteLine("\nSending Data... ");
                    _sendSocket.Send(b_Data, SocketFlags.None);
                    // User Message
                    Console.WriteLine("Sending Complete... " + iReceiveByteCount);
                    Console.WriteLine("\nGetting Data... ");
                    iReceiveByteCount = m_AcceptedSocket.Receive(ReceiveBuffer, SocketFlags.None);
                    response = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                    Console.WriteLine(response);
                    Console.WriteLine("\n Data Got ");
                }
            }

            private static string ParseBytes(byte[] ReceiveBuffer, int iReceiveByteCount)
            {
                return Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
            }

            private static Socket MSendSocket(string srvrIP, string srvrPort)
            {
                Socket m_sendSocket;
                m_sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress destinationIP = IPAddress.Parse(srvrIP);
                int destinationPort = System.Convert.ToInt16(srvrPort);
                IPEndPoint destinationEP = new IPEndPoint(destinationIP, destinationPort);
                Console.WriteLine("\nWaiting to Connect... ");
                m_sendSocket.Connect(destinationEP);
                return m_sendSocket;
            }

            private static byte[] TransformToBytes(string msg)
            {
                return System.Text.Encoding.ASCII.GetBytes(msg);
            }

            private static Socket InitListenSocket()
            {
                Socket m_ListenSocket;
                m_ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int iPort = 2084; // Set port number
                IPEndPoint m_LocalIPEndPoint = new IPEndPoint(IPAddress.Any, iPort);
                m_ListenSocket.Bind(m_LocalIPEndPoint);
                m_ListenSocket.Listen(4);
                return m_ListenSocket;
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