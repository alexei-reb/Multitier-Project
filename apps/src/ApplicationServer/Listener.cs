using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ApplicationServer
{
    public class Listener
    {
        private bool runFlag;
        private TcpListener tcpListener;

        public Listener(IPEndPoint endPoint)
        {
            tcpListener = new TcpListener(endPoint);
        }

        ~Listener()
        {
            Stop();
        }

        public void Start()
        {
            runFlag = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run));
            System.Diagnostics.Debug.Print("Listener: Listener started.");
        }

        public void Stop()
        {
            runFlag = false;
            System.Diagnostics.Debug.Print("Listener: Listener stopped.");
            tcpListener.Stop();
        }

        private void Run(object state)
        {
            tcpListener.Start();
            System.Diagnostics.Debug.Print("TcpListener started on {0}.", tcpListener.LocalEndpoint.ToString());
            while (this.runFlag)
            {
                try
                {
                    NewConnection(tcpListener.AcceptTcpClient(), null);
                    System.Diagnostics.Debug.Print("Listener: New client has been connected.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("Listener: Exception maessage. {0}", ex.Message);
                }
            }
        }

        public event EventHandler NewConnection;
    }
}
