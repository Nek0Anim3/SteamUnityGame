using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UIManager.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class SteamConnectionService : INetConnectionService, INetPolling
    {
        private readonly NetworkRoot _root;
        private Lobby _lobby;
        
        private bool _isDisconnecting;
        public SteamConnectionService(NetworkRoot root)
        {
            if (!SteamClient.IsValid)
            { 
                SteamClient.Init(480);    
            }
            
            _root = root;
            _root.NetworkManager.NetworkConfig.NetworkTransport = _root.FacepunchTransport;
            
            SteamFriends.OnGameLobbyJoinRequested += OnLobbyJoinRequested;
            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
            SteamMatchmaking.OnLobbyMemberLeave += OnLobbyLeave;
            _root.UI.OnDisconnectRequested += Disconnect;
            _root.UI.OnHostRequested += Void_StartHosting;
        }

        public void Tick()
        {
            SteamClient.RunCallbacks();
        }

        private void OnLobbyLeave(Lobby lobby, Friend friend)
        {
            if (friend.Id == lobby.Owner.Id)
            {
                if (_isDisconnecting) return; 
                _isDisconnecting = true;
            
                _lobby.Leave();
                _lobby = default;
                _root.NetworkManager.Shutdown();
                _isDisconnecting = false;
                
            }
        }

        private async void Void_StartHosting()
        {
            bool success = await StartHosting();
            if (!success)
            {
                Debug.Log("Failed to start steam lobby"); 
            }
        }
        
        public async Task<bool> StartHosting()
        {
            if (!SteamClient.IsValid)
            {
                SteamClient.Init(480);
            }

            Lobby? lobby = await SteamMatchmaking.CreateLobbyAsync(4);

            if (!lobby.HasValue) return false;

            _lobby = lobby.Value;
            _lobby.SetPublic();
            _lobby.SetJoinable(true);
            
            _root.NetworkManager.StartHost();
            
           
            return true;
        }

        public Task<bool> StartClientConnection()
        {
            return Task.FromResult(true);
        }

        private async void OnLobbyJoinRequested(Lobby lobby, SteamId steamId)
        {
            await lobby.Join(); 
        }
        
        private void OnLobbyEntered(Lobby lobby)
        {
            if (_root.NetworkManager.IsServer) return;
            _root.FacepunchTransport.targetSteamId = lobby.Owner.Id;
            _root.NetworkManager.StartClient();
            MenuManager.Instance.ChangeMenuPage(2);    
        }

        public void Disconnect()
        {
            if (_isDisconnecting) return; 
            _isDisconnecting = true;
            
            _lobby.Leave();
            _lobby = default;

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
            _isDisconnecting = false;
        }
    }
}