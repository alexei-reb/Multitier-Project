using System;
using System.Net.Sockets;
using ORM;
using System.Xml.Serialization;
using System.IO;
using ApplicationServer.Properties;
using System.Linq;
using System.Text;

namespace ApplicationServer
{
    public class ClientEvents
    {
        //private DAO dao = new DAO();
        ApplicationServer.Tests.DAOMock dao = new Tests.DAOMock();

        private JsonProtocol jsonProtocol = new JsonProtocol();

        public ClientEvents()
        {
            Exit += new EventHandler<SocketEventArgs>(ClientEvents_Exit);
            FullData += new EventHandler<SocketEventArgs>(ClientEvents_FullData);
            UpdateRecord += new EventHandler<SocketEventArgs>(ClientEvents_UpdateRecord);
            DeleteRecord += new EventHandler<SocketEventArgs>(ClientEvents_DeleteRecord);
            AddRecord += new EventHandler<SocketEventArgs>(ClientEvents_AddRecord);
            SendPhoto += new EventHandler<SocketEventArgs>(ClientEvents_Photo);
            SendPhotoLink += new EventHandler<SocketEventArgs>(ClientEvents_PhotoLink);
            ReadPhoto += new EventHandler<SocketEventArgs>(ClientEvents_ReadPhoto);
            SendFile += new EventHandler<SocketEventArgs>(ClientEvents_SendFile);
            ReadFile += new EventHandler<SocketEventArgs>(ClientEvents_ReadFile);
            ReadPhotoLink += new EventHandler<SocketEventArgs>(ClientEvents_ReadPhotoLink);
        }

        public void FireEvent(ClientThread clientThread, Command command)
        {
            System.Diagnostics.Debug.WriteLine("ClientEvents: New message from client: {0}, {1}", command.CurrentCommand, command.Data);

            switch (command.CurrentCommand)
            {
                case "Exit":
                    Exit(clientThread, new SocketEventArgs(command));
                    break;
                case "FullData":
                    FullData(clientThread, new SocketEventArgs(command));
                    break;
                case "UpdateRecord":
                    UpdateRecord(clientThread, new SocketEventArgs(command));
                    break;
                case "DeleteRecord":
                    DeleteRecord(clientThread, new SocketEventArgs(command));
                    break;
                case "AddRecord":
                    AddRecord(clientThread, new SocketEventArgs(command));
                    break;
                case "SendPhoto":
                    SendPhoto(clientThread, new SocketEventArgs(command));
                    break;
                case "SendPhotoLink":
                    SendPhotoLink(clientThread, new SocketEventArgs(command));
                    break;
                case "ReadPhoto":
                    ReadPhoto(clientThread, new SocketEventArgs(command));
                    break;
                case "SendFile":
                    SendFile(clientThread, new SocketEventArgs(command));
                    break;
                case "ReadFile":
                    ReadFile(clientThread, new SocketEventArgs(command));
                    break;
                case "ReadPhotoLink":
                    ReadPhotoLink(clientThread, new SocketEventArgs(command));
                    break;

            }
        }

        #region Events
        private event EventHandler<SocketEventArgs> Exit;
        private event EventHandler<SocketEventArgs> FullData;
        private event EventHandler<SocketEventArgs> UpdateRecord;
        private event EventHandler<SocketEventArgs> DeleteRecord;
        private event EventHandler<SocketEventArgs> AddRecord;
        private event EventHandler<SocketEventArgs> SendPhoto;
        private event EventHandler<SocketEventArgs> SendPhotoLink;
        private event EventHandler<SocketEventArgs> ReadPhoto;
        private event EventHandler<SocketEventArgs> SendFile;
        private event EventHandler<SocketEventArgs> ReadFile;
        private event EventHandler<SocketEventArgs> ReadPhotoLink;
        #endregion

        #region Private Methods
        private void ClientEvents_Exit(object sender, SocketEventArgs e)
        {
            ClientThread clientThread = sender as ClientThread;
            clientThread.Stop();
            Command command = new Command(Command.Commands.Confirm, string.Empty);

            jsonProtocol.SendObject(command, clientThread.Socket);
        }

        private void ClientEvents_FullData(object sender, SocketEventArgs e)
        {
            Table table = dao.GetQuery(e.Command.Data);
            Command command = new Command(Command.Commands.FullData, string.Empty);
            command.TypesList = table.TypesList;
            command.ValuesList = table.ValuesList;
            command.ColumnsList = table.ColumnsList;

            jsonProtocol.SendObject(command, (sender as ClientThread).Socket);
        }

        private void ClientEvents_UpdateRecord(object sender, SocketEventArgs e)
        {
            Table table = new Table();
            table.TypesList = e.Command.TypesList;
            table.ValuesList = e.Command.ValuesList;
            dao.UpdatePerson(int.Parse(e.Command.ValuesList.First().First().ToString()), e.Command.ValuesList.First());
        }

        private void ClientEvents_DeleteRecord(object sender, SocketEventArgs e)
        {
            dao.DeletePerson(e.Command.Data);
        }

        private void ClientEvents_AddRecord(object sender, SocketEventArgs e)
        {
            Table table = new Table();
            dao.AddPerson(e.Command.ValuesList);
        }

        private void ClientEvents_PhotoLink(object sender, SocketEventArgs e)
        {
            using (FileStream fs = new FileStream(e.Command.Data + "_" + e.Command.FilePath, FileMode.Create))
            {
                int bufferSize = int.Parse(Resources.Buffer);
                int position = 0;
                while (position < e.Command.File.Length)
                {
                    fs.Write(e.Command.File, position, bufferSize);
                    position += bufferSize;
                }
                fs.Write(e.Command.File, position - bufferSize, e.Command.File.Length - position);
            }
            dao.SetPhotoLinkName(e.Command.FilePath, int.Parse(e.Command.Data));
        }

        private void ClientEvents_Photo(object sender, SocketEventArgs e)
        {
            dao.AddPhoto(e.Command.File, int.Parse(e.Command.Data));
        }

        private void ClientEvents_ReadPhoto(object sender, SocketEventArgs e)
        {
            byte[] photo = dao.GetPhoto(int.Parse(e.Command.Data));

            Command command = new Command(Command.Commands.ReadPhoto, string.Empty);
            command.File = photo;
            command.FilePath = e.Command.FilePath;
            jsonProtocol.SendObject(command, (sender as ClientThread).Socket);
        }

        private void ClientEvents_SendFile(object sender, SocketEventArgs e)
        {
            dao.SaveFile(int.Parse(e.Command.Data), e.Command.File);
        }

        private void ClientEvents_ReadFile(object sender, SocketEventArgs e)
        {
            byte[] file = dao.GetFile(int.Parse(e.Command.Data));
            Command command = new Command(Command.Commands.ReadFile, string.Empty);
            command.File = file;
            command.FilePath = e.Command.FilePath;
            jsonProtocol.SendObject(command, (sender as ClientThread).Socket);
        }

        private void ClientEvents_ReadPhotoLink(object sender, SocketEventArgs e)
        {
            using (FileStream fs = new FileStream(e.Command.Data + "_" + e.Command.FilePath, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                Command command = new Command(Command.Commands.ReadPhotoLink, string.Empty);
                command.File = buffer;
                command.FilePath = e.Command.FilePath;
                jsonProtocol.SendObject(command, (sender as ClientThread).Socket);
            }
        }

        #endregion
    }
}
