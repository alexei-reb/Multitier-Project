using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Net;

namespace DesktopClient
{
    public class XMLProtocol
    {
        Messager messager = new Messager();

        public void SendObject(object command, Socket socket)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(command.GetType());
            MemoryStream stream = new MemoryStream();
            string message = string.Empty;
            xmlSerializer.Serialize(stream, command);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            message = Encoding.UTF8.GetString(buffer);
            messager.SendMessage(socket, message);
        }

        public object ReadObject(Socket socket, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            string message = messager.ReadMessage(socket);
            StringReader stringReader = new StringReader(message);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            return xmlSerializer.Deserialize(xmlTextReader);
        }

        public void Connect(IPEndPoint endPoint, Socket socket)
        {
            messager.Connect(endPoint, socket);
        }
    }
}
