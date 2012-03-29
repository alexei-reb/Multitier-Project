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
    public class TestsServer
    {
        #region SeuUp
        private IPEndPoint endPoint;
        private DesktopClient.JsonProtocol protocol;
        private Socket desktopSocket;

        [SetUp]
        public void Setup()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);
            protocol = new DesktopClient.JsonProtocol();
            desktopSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region Server
        [Test]
        public void ServerTest()
        {
            Server server = new Server(new Listener(new IPEndPoint(IPAddress.Any, 8888)), new ClientsManager());
            server.Start();
            server.Stop();
            Assert.Pass();
        }
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ServerWrongPort()
        {
            Server server = new Server(new Listener(new IPEndPoint(IPAddress.Any, -1)), new ClientsManager());
        }
        [Test]
        [ExpectedException(typeof(SocketException))]
        public void ServerWrongIP()
        {
            Server server = new Server(new Listener(new IPEndPoint(IPAddress.Parse("8.8.8.8"), 10060)), new ClientsManager());
            server.Start();
            server.Stop();
            Assert.Pass();
        }
        #endregion

        #region Listener
        [Test]
        public void ListenerAddClient()
        {
            Listener listener = new Listener(endPoint);
            listener.Start();
            protocol.Connect(endPoint, ref desktopSocket);
            Thread.Sleep(1000);
            listener.Stop();
            Assert.Pass();
        }
        #endregion

        #region ClientManager
        [Test]
        public void AddClientConnect()
        {
            Listener listener = new Listener(endPoint);
            ClientsManager clientManager = new ClientsManager();
            listener.NewConnection += clientManager.AddClient;
            listener.Start();
            clientManager.Start();

            protocol.Connect(endPoint, ref desktopSocket);

            Thread.Sleep(2000);
            clientManager.Stop();
            listener.Stop();
            Assert.Pass();
        }
        [Test]
        public void RemoveClient()
        {
            Listener listener = new Listener(endPoint);
            ClientsManager clientManager = new ClientsManager();
            Server server = new Server(listener, clientManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);
            //не подтягивается Json.dll
            protocol.SendObject(new Command(Command.Commands.Exit, string.Empty), desktopSocket);

            Command command = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;
            server.Stop();
            Assert.AreEqual("Confirm", command.CurrentCommand);
        }
        #endregion

        #region ClientEvents
        [Test]
        public void ClientFullData()
        {
            Listener listener = new Listener(endPoint);
            ClientsManager clientsManager = new ClientsManager();
            Server server = new Server(listener, clientsManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);
            protocol.SendObject(new Command(Command.Commands.FullData, string.Empty), desktopSocket);
            Command com = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            server.Stop();
            Assert.AreEqual("FullData", com.CurrentCommand);
        }

        [Test]
        public void ClientFullDataEmpty()
        {
            Listener listener = new Listener(endPoint);
            ClientsManager clientsManager = new ClientsManager();
            Server server = new Server(listener, clientsManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);
            protocol.SendObject(new Command(Command.Commands.FullData, string.Empty), desktopSocket);
            Command com = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            server.Stop();
            Assert.AreEqual(0, com.ValuesList.Count);
        }

        [Test]
        public void ClientFullDataOneEntry()
        {
            Listener listener = new Listener(endPoint);
            ClientsManager clientsManager = new ClientsManager();
            Server server = new Server(listener, clientsManager);
            server.Start();

            protocol.Connect(endPoint, ref desktopSocket);

            Command command = new Command(Command.Commands.AddRecord, string.Empty);
            command.ValuesList.Add(GetPerson());
            protocol.SendObject(command, desktopSocket);

            protocol.SendObject(new Command(Command.Commands.FullData, string.Empty), desktopSocket);
            Command com = protocol.ReadObject(desktopSocket, typeof(Command)) as Command;

            server.Stop();
            Assert.AreEqual(1, com.ValuesList.Count);
        }
        #endregion

        #region Custom
        private List<object> GetPerson()
        {
            Random random = new Random();
            int id = random.Next();
            byte[] photo = new byte[12];
            return new List<object>() 
            {
                id, 
                "FName" + id,
                "LName" + id,
                DateTime.Now,
                photo,
                "photo.jpg",
                "Email" + id,
                "+380" + id,
                "Description" + id,
                photo
            };
        }
        #endregion
    }
}
