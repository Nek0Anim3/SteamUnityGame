using System.Threading.Tasks;
using UIManager.MainMenu;

namespace Network
{
    public class DirectConnectionService : INetConnectionService
    {
        private readonly NetworkRoot _root;
        
        public DirectConnectionService(NetworkRoot root)
        {
            _root = root;
            _root.NetworkManager.NetworkConfig.NetworkTransport = _root.UnityTransport;
            
            _root.UI.OnHostRequested += Void_StartHost;
            _root.UI.OnClientRequested += Void_ConnectClient;
            _root.UI.OnDisconnectRequested += Disconnect;
        }
        
        public Task<bool> StartHosting()
        {

            _root.UnityTransport.SetConnectionData("127.0.0.1", 7777, "0.0.0.0");
            bool success = _root.NetworkManager.StartHost();

            return Task.FromResult(success);
            
        }

        public Task<bool> StartClientConnection()
        {
            _root.UnityTransport.SetConnectionData("127.0.0.1", 7777);
            bool success = _root.NetworkManager.StartClient();
            
            return Task.FromResult(success);
            
        }

        
        
        public async void Void_StartHost()
        {
            if (_root.NetworkManager.ShutdownInProgress)
            {
                return;
            }
            bool success = await StartHosting();
            
            if (!success)
            {
                UnityEngine.Debug.LogWarning("Failed to start hosting");
                return;
            }
            /*MenuManager.Instance.ChangeMenuPage(2);*/
            UnityEngine.Debug.Log("Host started!!");
        }

        public async void Void_ConnectClient()
        {
            await StartClientConnection();
            
            UnityEngine.Debug.Log("Client connected");
        }

        public void Disconnect()
        {
            if (_root.NetworkManager.IsServer && !_root.NetworkManager.IsClient) 
            {
                foreach (var clientId in _root.NetworkManager.ConnectedClientsIds)
                    _root.NetworkManager.DisconnectClient(clientId);
        
                _root.NetworkManager.Shutdown();
                return;
            }
    
            if (_root.NetworkManager.IsClient && !_root.NetworkManager.IsServer) 
            {
                _root.NetworkManager.Shutdown(); 
                return;
            }
    
            if (_root.NetworkManager.IsHost) 
            {
                _root.NetworkManager.Shutdown(); 
            }
        }
    }
}