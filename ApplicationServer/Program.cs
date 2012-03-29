using System;
using System.Linq;
using System.Net;

namespace ApplicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            IPEndPoint endPoint = new IPEndPoint(iphostentry.AddressList.First(), 8008);
            Listener listener = new Listener(endPoint);
            ClientsManager clientManager = new ClientsManager();
            Server server = new Server(listener, clientManager);
            server.Start();

            Console.WriteLine("Hosted on {0}\nPress any key to stop the server.", endPoint.ToString());
            Console.ReadKey();
        }
    }
}
