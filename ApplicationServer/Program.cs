using System;
using System.Net;

namespace ApplicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);
            Listener listener = new Listener(endPoint);
            ClientsManager clientManager = new ClientsManager();
            Server server = new Server(listener, clientManager);
            server.Start();

            Console.WriteLine("Hosted on {0}\nPress any key to stop the server.", endPoint.ToString());
            Console.ReadKey();
        }
    }
}
