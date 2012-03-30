using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MobileClient
{
    public class Messager
    {
        //fields
        static ManualResetEvent clientDone = new ManualResetEvent(false);
        SocketAsyncEventArgs socketEventArgs;

        //methods
        public void Connect(ref Socket socket, IPEndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketEventArgs = new SocketAsyncEventArgs();
            socketEventArgs.RemoteEndPoint = endPoint;
            socketEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
                delegate(object sender, SocketAsyncEventArgs e)
                {
                    clientDone.Set();
                });
            clientDone.Reset();
            socket.ConnectAsync(socketEventArgs);
            clientDone.WaitOne(5000);
        }

        public void SendMessage(Socket socket, string message)
        {

            socketEventArgs = new SocketAsyncEventArgs();
            socketEventArgs.RemoteEndPoint = socket.RemoteEndPoint;
            socketEventArgs.UserToken = null;
            socketEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(
                delegate(object sender, SocketAsyncEventArgs e)
                {
                    clientDone.Set();
                });
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            socketEventArgs.SetBuffer(byteMessage, 0, byteMessage.Length);
            clientDone.Reset();
            socket.SendAsync(socketEventArgs);
            clientDone.WaitOne();
        }

        public string ReadMessage(Socket socket)
        {
            message = string.Empty;
            socketEventArgs = new SocketAsyncEventArgs();
            socketEventArgs.RemoteEndPoint = socket.RemoteEndPoint;
            socketEventArgs.SetBuffer(new Byte[2048], 0, 2048);
            socketEventArgs.Completed += OnReadCompleted;
            clientDone.Reset();
            socket.ReceiveAsync(socketEventArgs);
            clientDone.WaitOne();
            return message;
        }

        private void OnReadCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                message = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
            }
            clientDone.Set();
        }

        private static string message;
    }
}
