using System.Net.Sockets;
using System;

namespace MobileClient
{
    public class ServerEventsHandler
    {
        public void FireEvent(Command command)
        {
            System.Diagnostics.Debug.WriteLine("Client: New message received: {0}", command.CurrentCommand);
            switch (command.CurrentCommand)
            {
                case "FullData":
                    FullData(null, new SocketEventArgs(command));
                    break;
                case "ReadPhoto":
                    ReadPhoto(null, new SocketEventArgs(command));
                    break;
                case "ReadFile":
                    ReadFile(null, new SocketEventArgs(command));
                    break;
                case "ReadPhotoLink":
                    ReadPhotoLink(null, new SocketEventArgs(command));
                    break;
            }
        }

        public event EventHandler<SocketEventArgs> FullData;
        public event EventHandler<SocketEventArgs> ReadPhoto;
        public event EventHandler<SocketEventArgs> ReadFile;
        public event EventHandler<SocketEventArgs> ReadPhotoLink;
    }
}
