using System.Net.Sockets;
using System.Threading;

namespace MobileClient
{
    public class Listener
    {
        private bool runFlag;
        private JsonProtocol xmlProtocol = new JsonProtocol();
        private Socket socket;
        private ServerEventsHandler serverEventsHandler = new ServerEventsHandler();

        public Listener(Socket socket, Presenter presenter)
        {
            this.socket = socket;
            serverEventsHandler.FullData += presenter.FillMainTable;
            serverEventsHandler.ReadPhoto += presenter.SaveFile;
            serverEventsHandler.ReadFile += presenter.SaveFile;
            serverEventsHandler.ReadPhotoLink += presenter.SetImage;
        }

        public void Start()
        {
            runFlag = true;
            ThreadPool.QueueUserWorkItem(Run);
        }

        public void Stop()
        {
            runFlag = false;
        }

        private void Run(object state)
        {
            while (runFlag)
            {
                Command command = xmlProtocol.ReadObject(socket, typeof(Command)) as Command;
                if (command != null)
                serverEventsHandler.FireEvent(command);

                Thread.Sleep(100);
            }
        }
    }
}
