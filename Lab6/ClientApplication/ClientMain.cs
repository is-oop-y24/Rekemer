using System;

namespace LAB6
{
   using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    class Program
    {
        static void Main(string[] args)
        {
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

            // User Message
            Console.WriteLine("Connected... ");

            //Send Information
            // Input data from user
            string msg = "";

            while (msg != "quit")
            {
                Console.Write("\nEnter Message to Send : ");
                msg = Console.ReadLine();
                byte[] b_Data = System.Text.Encoding.ASCII.GetBytes(msg);

                // User Message
                Console.WriteLine("\nSending Data... ");

                m_sendSocket.Send(b_Data, SocketFlags.None);

                // User Message
                Console.WriteLine("Sending Complete... ");
            }
            

        }
    }
}
}