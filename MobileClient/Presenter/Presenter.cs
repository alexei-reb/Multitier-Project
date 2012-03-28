using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MobileClient.Properties;
using System;

namespace MobileClient
{
    public class Presenter
    {
        #region Fields
        private Socket socket;
        private JsonProtocol protocol = new JsonProtocol();
        private Table table;
        private MainPage mainPage;
        private Listener listener;
        private int index = 0;
        #endregion

        #region Constructors
        public Presenter(MainPage mainPage)
        {
            this.mainPage = mainPage;
        }
        #endregion

        #region Properties
        public bool IsConnected
        {
            get
            {
                return socket.Connected;
            }
        }
        #endregion

        #region Client side Methods
        public bool Connect(string IP, int port)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Client: Connect.");
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                protocol.Connect(new IPEndPoint(IPAddress.Parse(IP), port), ref socket);
                if (socket.Connected)
                {
                    listener = new Listener(socket, this);
                    listener.Start();
                }
                else
                {
                    throw new SocketException();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Exit()
        {
            System.Diagnostics.Debug.WriteLine("WecClient: Exit.");
            if (socket != null)
            {
                Command command = new Command(Command.Commands.Exit, string.Empty);
                protocol.SendObject(command, socket);
                protocol.ReadObject(socket, typeof(Command));
            }
        }
        public void GetTable(string tableName)
        {
            System.Diagnostics.Debug.WriteLine("WecClient: Get table.");
            Command command = new Command(Command.Commands.FullData, tableName);
            protocol.SendObject(command, socket);
        }
        public void FillRow()
        {
            if (table != null)
            {
                index = index % table.ValuesList.Count;
                mainPage.Dispatcher.BeginInvoke(
                    new Action(
                        delegate()
                        {
                            mainPage.tboxID.Text = table.ValuesList[index][0] != null ? table.ValuesList[index][0].ToString() : string.Empty;
                            mainPage.tboxFName.Text = table.ValuesList[index][1] != null ? table.ValuesList[index][1].ToString() : string.Empty;
                            mainPage.tboxLName.Text = table.ValuesList[index][2] != null ? table.ValuesList[index][2].ToString() : string.Empty;
                            mainPage.tboxBDate.Text = table.ValuesList[index][3] != null ? table.ValuesList[index][3].ToString() : string.Empty;
                            mainPage.tboxEMail.Text = table.ValuesList[index][6] != null ? table.ValuesList[index][6].ToString() : string.Empty;
                            mainPage.tboxPhone.Text = table.ValuesList[index][7] != null ? table.ValuesList[index][7].ToString() : string.Empty;
                            mainPage.tboxDescription.Text = table.ValuesList[index][8] != null ? table.ValuesList[index][8].ToString() : string.Empty;
                        }));
            }
        }
        public void NextRow()
        {
            index++;
            FillRow();
            UpdateCurrnetTextBlock();
        }
        public void PrewRow()
        {
            index = index < 1 ? table.ValuesList.Count : index - 1;
            FillRow();
            UpdateCurrnetTextBlock();
        }
        private void UpdateCurrnetTextBlock()
        {
            mainPage.Dispatcher.BeginInvoke(new Action(
            delegate()
            {
                mainPage.tbInfoCurrnet.Text = (index + 1).ToString();
            }));
        }
        public void UpdatePerson(List<object> values)
        {
            System.Diagnostics.Debug.WriteLine("WecClient: Update person.");
            Command command = new Command(Command.Commands.UpdateRecord, string.Empty);
            command.ValuesList.Add(values);
            protocol.SendObject(command, socket);
        }

        public void AddPerson(List<object> values)
        {
            System.Diagnostics.Debug.WriteLine("WecClient: Add person.");
            Command command = new Command(Command.Commands.AddRecord, string.Empty);
            command.ValuesList.Add(values);
            protocol.SendObject(command, socket);
        }
        public void DeletePerson(object id)
        {
            System.Diagnostics.Debug.WriteLine("WecClient: Delete person.");
            Command command = new Command(Command.Commands.DeleteRecord, id.ToString());
            protocol.SendObject(command, socket);
        }
        public void SendPhoto(string fileName, object ID)
        {
            Command command = new Command(Command.Commands.SendPhoto, ID.ToString());
            //command.File = ConvertPhoto(fileName);
            protocol.SendObject(command, socket);
        }
        public void ReadPhoto(string fileName, object id)
        {
            Command command = new Command(Command.Commands.ReadPhoto, id.ToString());
            command.FilePath = fileName;
            protocol.SendObject(command, socket);
        }
        public void SendPhotoLink(string fileName, object ID)
        {
            Command command = new Command(Command.Commands.SendPhotoLink, ID.ToString());
            //command.File = new Imager().ImageToBytes(fileName);//read file
            command.FilePath = Path.GetFileName(fileName);
            protocol.SendObject(command, socket);
        }
        public void ReadPhotoLink(string fileName, object id)
        {
            Command command = new Command(Command.Commands.ReadPhotoLink, id.ToString());
            command.FilePath = fileName;
            protocol.SendObject(command, socket);
        }
        public void SendFile(string fileName, object id)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                Command command = new Command(Command.Commands.SendFile, id.ToString());
                command.File = buffer;
                command.FilePath = Path.GetFileName(fileName);
                protocol.SendObject(command, socket);
            }


        }
        public void ReadFile(string fileName, object id)
        {
            Command command = new Command(Command.Commands.ReadFile, id.ToString());
            command.FilePath = fileName;
            protocol.SendObject(command, socket);
        }
        #endregion

        #region Server side Methods
        public void FillMainTable(object sender, SocketEventArgs e)
        {
            table = new Table();
            table.ColumnsList = e.Command.ColumnsList;
            table.TypesList = e.Command.TypesList;
            table.ValuesList = e.Command.ValuesList;
            mainPage.Dispatcher.BeginInvoke(new Action(
                delegate()
                {
                    mainPage.tbInfoTotalCount.Text = table.ValuesList.Count.ToString();
                }));
            UpdateCurrnetTextBlock();
        }
        public void SaveFile(object sender, SocketEventArgs e)
        {
            using (FileStream fs = new FileStream(e.Command.FilePath, FileMode.Create))
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
        }
        //stub (Ваш К.О.)
        public EventHandler<SocketEventArgs> SetImage { get; set; }
        #endregion

        
    }
}