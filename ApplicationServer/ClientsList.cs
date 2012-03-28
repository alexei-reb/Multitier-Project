using System.Collections.Generic;
using System.Net.Sockets;

namespace ApplicationServer
{
    public class ClientsList
    {
        private List<ClientThread> clientList = new List<ClientThread>();

        public void Add(ClientThread clientThread)
        {
            clientList.Add(clientThread);
        }

        public bool Contains(TcpClient tcpClient)
        {
            bool result = false;
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].Socket.GetHashCode() == tcpClient.GetHashCode())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void Remove(ClientThread clientThread)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                if (clientList[i].GetHashCode() == clientThread.GetHashCode())
                {
                    clientList[i].Socket.Close();
                    clientList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
