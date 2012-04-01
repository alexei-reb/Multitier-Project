using System;
using System.Net.Sockets;

namespace ApplicationServer
{
    public class ClientsManager
    {
        #region Fields
        private ClientsList clientList = new ClientsList();
        #endregion

        #region Properties
        public static bool RunFlag { get; set; }
        #endregion

        #region Methods
        public void AddClient(object sender, EventArgs e)
        {
            //BUG: One client can connect multiply times.
            System.Diagnostics.Debug.Print("ClientsManager: Client trying to connect.");
            ClientThread clientThread = new ClientThread(sender as TcpClient);
            clientList.Add(clientThread);
            clientThread.ThreadStop += client_ThreadStop;
            clientThread.Start();
            System.Diagnostics.Debug.Print("ClientsManager: Connect accepted.");
        }

        public void Start()
        {
            RunFlag = true;
            System.Diagnostics.Debug.Print("ClientsManager: ClientManager started.");
        }

        public void Stop()
        {
            RunFlag = false;
            System.Diagnostics.Debug.Print("ClientsManager: ClientManager stopped.");
        }

        private void client_ThreadStop(object sender, EventArgs e)
        {
            ClientThread client = sender as ClientThread;

            if (clientList.Contains(client.Socket))
            {
                clientList.Remove(client);
                System.Diagnostics.Debug.Print("ClientsManager: client removed.");
            }
        }
        #endregion
    }
}
