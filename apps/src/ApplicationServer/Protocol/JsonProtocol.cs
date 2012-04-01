using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using ApplicationServer.Properties;

namespace ApplicationServer
{
    public class JsonProtocol
    {
        public void SendObject(object command, TcpClient tcpClient)
        {
            string message = JsonConvert.SerializeObject(command);
            byte[] messageButes = Encoding.UTF8.GetBytes(message);
            tcpClient.GetStream().Write(messageButes, 0, messageButes.Length);
        }

        public object ReadObject(TcpClient tcpClient, Type type)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[int.Parse(Resources.Buffer)];

            while (tcpClient.Available > 0)
            {
                int bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            string mesg = sb.ToString();

            return JsonConvert.DeserializeObject(sb.ToString(), type);
        }
    }
}
