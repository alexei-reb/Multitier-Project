using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace ApplicationServer.Tests
{
    [TestFixture]
    public class Tests
    {
        Listener listener;
        IPEndPoint endPoint;
        ClientsManager clientManager;

        [SetUp]
        public void Setup()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);
            listener = new Listener(endPoint);
            clientManager = new ClientsManager();
        }

        [Test]
        [ExpectedException(typeof(SocketException))]
        public void ConnectEx()
        {
            Listener listener1 = new Listener(endPoint);
            Listener listener2 = new Listener(endPoint);
            ClientsManager clientManager1 = new ClientsManager();
            ClientsManager clientManager2 = new ClientsManager();
            Server server1 = new Server(listener1, clientManager1);
            Server server2 = new Server(listener2, clientManager2);
            server1.Start();
            server2.Start();
            Thread.Sleep(1000);
        }

        [Test]
        public void ServerStartStopTest()
        {
            Server server = new Server(listener, clientManager);
            server.Start();
            server.Stop();

            Server server1 = new Server(listener, clientManager);
            server1.Start();
            server1.Stop();

            Assert.Pass(); 
        }

        [Test]
        public void ClientConnectAndExit()
        {
            Server server = new Server(listener, clientManager);
            server.Start();

            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(endPoint);

            new JsonProtocol().SendObject(new Command(Command.Commands.Exit, string.Empty), tcpClient);
            Command command = new JsonProtocol().ReadObject(tcpClient, typeof(Command)) as Command;

            Assert.AreEqual("Confirm", command.CurrentCommand);
        }
    }
}
