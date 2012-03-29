using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationServer;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestsOnly
{
    class Program
    {
        static void Main(string[] args)
        {
            DesktopClient.JsonProtocol protocol = new DesktopClient.JsonProtocol();
            Socket desktopSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);

            Listener listener = new Listener(endPoint);
            ClientsManager clientManager = new ClientsManager();
            Server server = new Server(listener, clientManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);

            protocol.SendObject(new Command(Command.Commands.Exit, string.Empty), desktopSocket);

            Command command = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;
            server.Stop();
        }
    }
}
