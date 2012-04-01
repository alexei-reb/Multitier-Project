using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using WebClient.Properties;

namespace WebClient
{
    public partial class ICallbackEventHandlerImplementor : Page, ICallbackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string webFormDoCallbackScript = this.ClientScript.GetCallbackEventReference(this, "arg", "onSuccessfullHandler", null, true);
                string serverCallScript = "function serverCall(arg){" + webFormDoCallbackScript + ";\n}\n";

                if (!this.ClientScript.IsClientScriptBlockRegistered("serverCallScript"))
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "serverCallScript", serverCallScript, true);
                }

            }
        }
        #region ICallbackEventHandler Members
        public string GetCallbackResult()
        {
            return result;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] argsFromClient = eventArgument.Split('|');

            switch (argsFromClient.First())
            {
                case "GetFullData":
                    Connect(argsFromClient[1], int.Parse(argsFromClient[2]));
                    GetFullData();
                    break;
                case "GetImage":
                    Connect(argsFromClient[1], int.Parse(argsFromClient[2]));
                    GetImage(int.Parse(argsFromClient[3]));
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Conversation vith server
        private TcpClient tcpClient = new TcpClient();
        private IPEndPoint endPoint;
        private string result;

        private bool IsConnected()
        {
            return tcpClient.Connected;
        }

        private void Connect(string ip, int port)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpClient.Connect(endPoint);
        }

        private void GetFullData()
        {
            Command command = new Command(Command.Commands.FullData, string.Empty);
            JsonProtocol protocol = new JsonProtocol();
            protocol.SendObject(command, tcpClient);
            Command comresp = null;
            while (comresp == null)
            {
                comresp = protocol.ReadObject(tcpClient, typeof(Command)) as Command;
            }
            protocol.SendObject(new Command(Command.Commands.Exit, string.Empty), tcpClient);
            result = MakeMeTable(comresp);
        }
        private string MakeMeTable(Command command)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table><tr>");
            for (int i = 0; i < command.ColumnsList.Count; i++)
            {
                stringBuilder.Append(string.Format("<th>{0}</th>", command.ColumnsList[i]));
            }
            stringBuilder.Append("</tr>");
            for (int i = 0; i < command.ValuesList.Count; i++)
            {
                stringBuilder.Append("<tr>");
                for (int j = 0; j < command.ColumnsList.Count; j++)
                {
                    stringBuilder.Append(string.Format("<td>{0}</td>", command.ValuesList[i][j]));
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        private void GetImage(int id)
        {
            Command command = new Command(Command.Commands.ReadPhoto, id.ToString());
            JsonProtocol protocol = new JsonProtocol();
            protocol.SendObject(command, tcpClient);

            Command respCom = null;
            while (respCom == null)
            {
                respCom = protocol.ReadObject(tcpClient, typeof(Command)) as Command;
            }
            string fileName = "tmp.jpg";
            SaveFile(respCom.File, fileName);
            result = string.Format(@"<img src=""{0}"">", result);
        }

        public void SaveFile(byte[] file, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                int bufferSize = int.Parse(Resources.Buffer);
                int position = 0;
                while (position < file.Length)
                {
                    fs.Write(file, position, bufferSize);
                    position += bufferSize;
                }
                fs.Write(file, position - bufferSize, file.Length - position);
                result = fs.Name;
            }
        }
        #endregion
    }
}