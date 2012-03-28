using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;

namespace ApplicationServer.Tests
{
    [TestFixture]
    public class Tests
    {
        Listener listener;
        IPEndPoint endPoint;

        [SetUp]
        public void Setup()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008);
            listener = new Listener(endPoint);
        }

        [Test]
        [ExpectedException]
        public void ConnectEx()
        {
            Server server = new Server(new Listener(endPoint), new ClientsManager());
            server.Start();
            Server server2 = new Server(new Listener(endPoint), new ClientsManager());
            server2.Start();
        }
    }
}
