using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ApplicationServer.Tests
{
    class TestDesktop
    {
        #region SeuUp
        private Listener listener;
        private IPEndPoint endPoint;
        private ClientsManager clientManager;
        private DesktopClient.JsonProtocol protocol;
        private Socket desktopSocket;

        [SetUp]
        public void Setup()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);
            listener = new Listener(endPoint);
            clientManager = new ClientsManager();
            protocol = new DesktopClient.JsonProtocol();
            desktopSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region Connect
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

            protocol.Connect(endPoint, ref desktopSocket);

            protocol.SendObject(new Command(Command.Commands.Exit, string.Empty), desktopSocket);
            Command command = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            Assert.AreEqual("Confirm", command.CurrentCommand);
        }
        #endregion

        #region FullData
        [Test]
        public void FullDataRedEmpty()
        {
            Server server = new Server(listener, clientManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);

            protocol.SendObject(new Command(Command.Commands.FullData, string.Empty), desktopSocket);
            Command command = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            Assert.AreEqual("FullData", command.CurrentCommand);
        }

        [Test]
        public void FullDataReadOne()
        {
            Server server = new Server(listener, clientManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);

            protocol.SendObject(new Command(Command.Commands.FullData, string.Empty), desktopSocket);
            Command command = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            Assert.AreEqual("FullData", command.CurrentCommand);
        }
        #endregion
    }
}
