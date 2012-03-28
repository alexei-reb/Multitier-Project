using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationServer;
using System.Net;
using System.Net.Sockets;

namespace TestsOnly
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

            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(endPoint);

            new JsonProtocol().SendObject(new Command(Command.Commands.Exit, string.Empty), tcpClient);

            Command command = new JsonProtocol().ReadObject(tcpClient, typeof(Command)) as Command;


            bool ResolveEventArgs = ("Confirm" == command.CurrentCommand);
        }
    }
}
