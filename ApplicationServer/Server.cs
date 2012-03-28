
namespace ApplicationServer
{
    public class Server
    {
        #region Fields
        private Listener listener;
        private ClientsManager clientManager;
        #endregion

        #region Constructors
        public Server(Listener listener, ClientsManager clientManager)
        {
            this.listener = listener;
            this.clientManager = clientManager;
            listener.NewConnection += clientManager.AddClient;
        }
        
        ~Server()
        {
            Stop();
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            listener.Start();
            clientManager.Start();
            System.Diagnostics.Debug.Print("Server: Server started.");
        }

        public void Stop()
        {
            clientManager.Stop();
            listener.Stop();
            System.Diagnostics.Debug.Print("Server: Server stopped.");
        }
        #endregion
    }
}
