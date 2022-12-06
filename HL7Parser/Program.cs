using System;
using System.Net;
using System.Threading;

namespace HL7Parser
{
    internal class Program
    {
        private static readonly byte[] Localhost = { 127, 0, 0, 1 };
        private const int Port = 7777;

        static void Main(string[] args)
        {
            IPAddress address = new IPAddress(Localhost);
            IPEndPoint endPoint = new IPEndPoint(address, Port);

            try
            {
                // Create a thread for listening to a port.
                Subscriber subscriber = new Subscriber(endPoint);
                Thread listnerThread = new Thread(new ThreadStart(subscriber.Listen));
                listnerThread.Start();

                // Craete another thread for sending HL7 messages
                // Send Message so that the listening port catches it.
                Publisher publisher = new Publisher(Localhost, Port);
                Thread senderThread = new Thread(new ThreadStart(publisher.Send));
                senderThread.Start();
            }
            catch (Exception e)
            {
                // Exception handling
                Console.WriteLine("An unexpected exception occured: {0}", e.Message);
            }
        }
    }
}
