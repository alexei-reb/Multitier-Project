using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DesktopClient.Properties;

namespace DesktopClient
{
    public class Messager
    {
        public Messager()
        {
            connectDone = new ManualResetEvent(false);
            sendDone = new ManualResetEvent(false);
            readDone = new ManualResetEvent(false);
            connectDone.Reset();
            sendDone.Reset();
            readDone.Reset();
        }

        #region Connect
        private ManualResetEvent connectDone;

        public void Connect(IPEndPoint endPoint, Socket client)
        {
            client.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();
        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            Socket client = asyncResult.AsyncState as Socket;
            client.EndConnect(asyncResult);
            connectDone.Set();
        }
        #endregion

        #region Send
        private ManualResetEvent sendDone;

        public void SendMessage(Socket client, string message)
        {
            byte[] byteMessage = Encoding.ASCII.GetBytes(message);
            client.BeginSend(byteMessage, 0, byteMessage.Length, SocketFlags.None, new AsyncCallback(SendCallBack), client);
            sendDone.WaitOne();
            sendDone.Reset();
        }

        private void SendCallBack(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;
            int byteSent = client.EndSend(result);
            sendDone.Set();
        }

        #endregion

        #region Read

        private static ManualResetEvent readDone;

        public class StateObject
        {
            public Socket workSocket = null;
            public static int BufferSize = int.Parse(Resources.Buffer);
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public string ReadMessage(Socket client)
        {
            StateObject state = new StateObject();
            state.workSocket = client;
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            readDone.WaitOne();
            readDone.Reset();
            return state.sb.ToString();
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            StateObject state = result.AsyncState as StateObject;
            Socket client = state.workSocket;
            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, state.buffer.Length));
            Thread.Sleep(1000);
            int bytesRead = 0;
            while (client.Available > 0)
            {
                bytesRead = client.Receive(state.buffer, 0, state.buffer.Length, SocketFlags.None);
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
            }
            readDone.Set();
        }
        #endregion
    }
}
