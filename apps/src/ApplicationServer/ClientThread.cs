using System;
using System.Net.Sockets;
using System.Threading;
using ApplicationServer.Properties;

namespace ApplicationServer
{
    public class ClientThread
    {
        #region Fields
        private bool runFlag;
        private ClientEvents clientEvents = new ClientEvents();
        private TcpClient tcpClient;
        private JsonProtocol protocol = new JsonProtocol();
        private System.Timers.Timer timer;
        private int timeOut;
        #endregion

        #region Constructors
        public ClientThread(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            timeOut = int.Parse(Resources.TimeOut);
            timer = new System.Timers.Timer(timeOut);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
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
            tcpClient.Close();
            System.Diagnostics.Debug.Print("ClientThread: Client thread stopped");
        }

        private void Run(object obj)
        {
            while (ClientsManager.RunFlag && runFlag)
            {
                if (tcpClient.Available > 0)
                {
                    timer.Interval = timeOut;
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

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            protocol.SendObject(new Command(Command.Commands.Timeout, string.Empty), tcpClient);
            System.Diagnostics.Debug.Print("Timeout!");
            Stop();
        }
        #endregion

        public event EventHandler ThreadStop;
    }
}
