using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using System.Text;
using System.IO;
using DesktopClient.Properties;

namespace DesktopClient
{
    public class Presenter
    {
        #region Fields
        private Socket socket;
        private JsonProtocol protocol = new JsonProtocol();
        private MainWindow mainWindow;
        private Listener listener;
        #endregion

        #region Constructors
        public Presenter(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        #endregion

        #region Client side Methods
        public void Connect()
        {
            System.Diagnostics.Debug.Print("Client: Connect.");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            protocol.Connect(new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8008), ref socket);
            listener = new Listener(socket, this);
            listener.Start();
        }
        public void Exit()
        {
            System.Diagnostics.Debug.Print("Client: Exit.");
            if (socket != null)
            {
                Command command = new Command(Command.Commands.Exit, string.Empty);
                protocol.SendObject(command, socket);
                protocol.ReadObject(socket, typeof(Command));
            }
        }
        public void GetTable(string tableName)
        {
            System.Diagnostics.Debug.Print("Client: Get table.");
            Command command = new Command(Command.Commands.FullData, tableName);
            protocol.SendObject(command, socket);
        }
        public void UpdatePerson(List<object> values)
        {
            System.Diagnostics.Debug.Print("Client: Update person.");
            Command command = new Command(Command.Commands.UpdateRecord, string.Empty);
            command.ValuesList.Add(values);
            protocol.SendObject(command, socket);
        }
        public void AddPerson(List<object> values)
        {
            System.Diagnostics.Debug.Print("Client: Add person.");
            Command command = new Command(Command.Commands.AddRecord, string.Empty);
            command.ValuesList.Add(values);
            protocol.SendObject(command, socket);
        }
        public void DeletePerson(object id)
        {
            System.Diagnostics.Debug.Print("Client: Delete person.");
            Command command = new Command(Command.Commands.DeleteRecord, id.ToString());
            protocol.SendObject(command, socket);
        }
        public void SendPhoto(string fileName, object ID)
        {
            Command command = new Command(Command.Commands.SendPhoto, ID.ToString());
            command.File = ConvertPhoto(fileName);
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
            command.File = ConvertPhoto(fileName);
            command.FilePath = Path.GetFileName(fileName);
            protocol.SendObject(command, socket);
        }
        public void ReadPhotoLink(string fileName, object id)
        {
            Command command = new Command(Command.Commands.ReadPhotoLink, id.ToString());
            command.FilePath = fileName;
            protocol.SendObject(command, socket);
        }
        private byte[] ConvertPhoto(string fileName)
        {
            BitmapSource bmp = PSFactory.GetInstance(fileName).Load();
            BinaryImageConverter c = new BinaryImageConverter();
            return (byte[])c.ConvertBack((BitmapImage)bmp, typeof(byte[]), null, null);
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
        public void FillDataGrid(object sender, SocketEventArgs e)
        {
            DataTable dataTable = CreateDataTable(e.Command.ColumnsList, e.Command.TypesList, e.Command.ValuesList);
            mainWindow.MainDataGrid.Dispatcher.Invoke(new Action(
                delegate()
                {
                    mainWindow.MainDataGrid.ItemsSource = dataTable.DefaultView;
                }));
        }
        private DataTable CreateDataTable(List<string> headers, List<string> types, List<List<object>> content)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < headers.Count; i++)
            {
                Type type = Type.GetType(types[i]);
                dataTable.Columns.Add(headers[i], type);
            }

            for (int i = 0; i < content.Count; i++)
            {
                object[] row = new object[dataTable.Columns.Count];
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row[j] = content[i][j];
                }
                DataRow dataRow = dataTable.NewRow();
                dataRow.ItemArray = row;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
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
        public void SetImage(object sender, SocketEventArgs e)
        {
            BinaryImageConverter converter = new BinaryImageConverter();
            mainWindow.MainDataGrid.Dispatcher.Invoke(new Action(
                delegate()
                {
                    mainWindow.MainImage.Source = converter.Convert(e.Command.File, typeof(byte[]), null, null) as BitmapImage; 
                }));
        }
        #endregion
    }
}