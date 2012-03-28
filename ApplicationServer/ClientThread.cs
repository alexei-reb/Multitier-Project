using System;
using System.Net.Sockets;
using System.Threading;

namespace ApplicationServer
{
    public class ClientThread
    {
        #region Fields
        private bool runFlag;
        private ClientEvents clientEvents = new ClientEvents();
        private TcpClient tcpClient;
        private JsonProtocol protocol = new JsonProtocol();
        #endregion

        #region Constructors
        public ClientThread(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }
        #endregion

        #region Properties
        public ClientEvents ClientEvents
        {
            set
            {
                clientEvents = value;
            }
        }
        public TcpClient Socket
        {
            get
            {
                return tcpClient;
            }
        }
        #endregion

        #region Methods
        public void Start()
        {
            runFlag = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run));
            System.Diagnostics.Debug.Print("ClientThread: Client thread started");
        }

        public void Stop()
        {
            runFlag = false;
            ThreadStop(this, null);
            System.Diagnostics.Debug.Print("ClientThread: Client thread stopped");
        }

        private void Run(object obj)
        {
            while (ClientsManager.RunFlag && runFlag)
            {
                if (tcpClient.Available > 0)
                {
                    try
                    {
                        Command command = protocol.ReadObject(tcpClient, typeof(Command)) as Command;
                        clientEvents.FireEvent(this, command);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print("Server: {0}", ex.Message);
                    }
                }
                Thread.Sleep(100);
            }
        }
        #endregion

        public event EventHandler ThreadStop;
    }
}
