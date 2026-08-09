using System.Threading.Tasks;
using FishNet.Managing.Transporting;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using UIManager.MainMenu;

namespace Network.Connection
{
    public class DirectConnectionService : INetConnectionService
    {
        private readonly NetworkRoot _root;
        
        public DirectConnectionService(NetworkRoot root)
        {
            _root = root;
            
            _root.UI.OnHostRequested += Void_StartHost;
            _root.UI.OnClientRequested += Void_ConnectClient;
            _root.UI.OnDisconnectRequested += Disconnect;
        }
        
        public Task<bool> StartHosting()
        {
            Tugboat tugboat = _root.NetworkManager.TransportManager.GetTransport<Tugboat>();
            tugboat.SetServerBindAddress("127.0.0.1", IPAddressType.IPv4);
            tugboat.SetPort(7777);
            bool success = _root.NetworkManager.ServerManager.StartConnection();

            return Task.FromResult(success);
            
        }

        public Task<bool> StartClientConnection()
        {
            Tugboat tugboat = _root.NetworkManager.TransportManager.GetTransport<Tugboat>();
            tugboat.SetServerBindAddress("127.0.0.1", IPAddressType.IPv4);
            tugboat.SetPort(7777);
            bool success = _root.NetworkManager.ClientManager.StartConnection();
            
            return Task.FromResult(success);
            
        }

        
        
        public async void Void_StartHost()
        {
            if (!_root.NetworkManager.IsOffline)
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
            if (_root.NetworkManager.IsServerStarted && !_root.NetworkManager.IsHostStarted) 
            {
                foreach (var client in _root.NetworkManager.ServerManager.Clients.Values)
                {
                    client.Disconnect(true);
                }
                _root.NetworkManager.ServerManager.StopConnection(true);
                return;
            }
    
            if (_root.NetworkManager.IsClientStarted && !_root.NetworkManager.IsHostStarted) 
            {
                _root.NetworkManager.ClientManager.StopConnection();
                return;
            }
        }
    }
}