using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using DesktopClient.Properties;
using System.Net;

namespace DesktopClient
{
    public class JsonProtocol
    {
        private Messager messager = new Messager();

        public void SendObject(object command, Socket socket)
        {
            string message = JsonConvert.SerializeObject(command);
            byte[] messageButes = Encoding.UTF8.GetBytes(message);

            messager.SendMessage(socket, message);
        }

        public object ReadObject(Socket socket, Type type)
        {
            string message = messager.ReadMessage(socket);
            return JsonConvert.DeserializeObject(message, type);
        }

        public void Connect(IPEndPoint endPoint, ref Socket socket)
        {
            messager.Connect(endPoint, socket);
        }
    }
}
