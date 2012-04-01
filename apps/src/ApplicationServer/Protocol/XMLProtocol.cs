using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace ApplicationServer
{
    public class XMLProtocol
    {
        public void SendObject(object command, TcpClient tcpClient)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(command.GetType());
            xmlSerializer.Serialize(tcpClient.GetStream(), command);
        }

        public object ReadObject(TcpClient tcpClient, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            return xmlSerializer.Deserialize(tcpClient.GetStream());
        }
    }
}
