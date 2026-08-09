using System.Threading.Tasks;
using FishNet.Managing;
using Steamworks;
using UIManager.MainMenu;
using UnityEngine;

namespace Network
{
    public class SteamConnectionService : INetConnectionService, INetPolling
    {
        private readonly NetworkRoot _root;
        private readonly NetworkManager _nm;

        private CSteamID _lobbyId = CSteamID.Nil;
        private bool _steamInitialized;
        private bool _isDisconnecting;
        private bool _isStartingHost; 

        private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
        private Callback<LobbyEnter_t> _lobbyEntered;
        private Callback<LobbyChatUpdate_t> _lobbyChatUpdate;
        private CallResult<LobbyCreated_t> _lobbyCreatedResult;

        private TaskCompletionSource<bool> _createLobbyTcs;

        public SteamConnectionService(NetworkRoot root)
        {
            _root = root;
            _nm = _root.NetworkManager;

            if (!_steamInitialized)
                _steamInitialized = SteamAPI.Init();

            _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            _lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
            _lobbyCreatedResult = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);

            _root.UI.OnDisconnectRequested += Disconnect;
            _root.UI.OnHostRequested += Void_StartHosting;
        }

        public void Tick()
        {
            if (_steamInitialized)
                SteamAPI.RunCallbacks();
        }

        private async void Void_StartHosting()
        {
            bool success = await StartHosting();
            if (!success)
                Debug.Log("Failed to start steam lobby");
        }

        public async Task<bool> StartHosting()
        {
            if (!_steamInitialized)
            {
                _steamInitialized = SteamAPI.Init();
                if (!_steamInitialized) return false;
            }

            _isStartingHost = true;
            _createLobbyTcs = new TaskCompletionSource<bool>();

            SteamAPICall_t handle = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
            _lobbyCreatedResult.Set(handle);

            bool created = await _createLobbyTcs.Task;

            if (!created)
            {
                _isStartingHost = false;
                return false;
            }

            SteamMatchmaking.SetLobbyJoinable(_lobbyId, true);

            _nm.ServerManager.StartConnection();
            _nm.ClientManager.StartConnection();

            _isStartingHost = false;

            if (!_nm.IsServerStarted)
            {
                SteamMatchmaking.LeaveLobby(_lobbyId);
                _lobbyId = CSteamID.Nil;
                return false;
            }

            return true;
        }

        private void OnLobbyCreated(LobbyCreated_t result, bool ioFailure)
        {
            if (ioFailure || result.m_eResult != EResult.k_EResultOK)
            {
                _createLobbyTcs?.TrySetResult(false);
                return;
            }

            _lobbyId = new CSteamID(result.m_ulSteamIDLobby);
            _createLobbyTcs?.TrySetResult(true);
        }

        public Task<bool> StartClientConnection()
        {
            return Task.FromResult(true);
        }
        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t data)
        {
            SteamMatchmaking.JoinLobby(data.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t data)
        {
            _lobbyId = new CSteamID(data.m_ulSteamIDLobby);

            if (_isStartingHost) return; 

            CSteamID hostId = SteamMatchmaking.GetLobbyOwner(_lobbyId);
            _nm.ClientManager.StartConnection(hostId.m_SteamID.ToString());

            MenuManager.Instance.ChangeMenuPage(2);
        }

        private void OnLobbyChatUpdate(LobbyChatUpdate_t data)
        {
            var lobbyId = new CSteamID(data.m_ulSteamIDLobby);
            if (lobbyId != _lobbyId) return;

            var stateChange = (EChatMemberStateChange)data.m_rgfChatMemberStateChange;
            bool leftOrDisconnected = (stateChange & (EChatMemberStateChange.k_EChatMemberStateChangeLeft | EChatMemberStateChange.k_EChatMemberStateChangeDisconnected | EChatMemberStateChange.k_EChatMemberStateChangeKicked)) != 0;
            if (!leftOrDisconnected) return;

            var changedUser = new CSteamID(data.m_ulSteamIDUserChanged);
            CSteamID owner = SteamMatchmaking.GetLobbyOwner(_lobbyId);

            if (changedUser != owner) return;
            if (_isDisconnecting) return;

            _isDisconnecting = true;
            _lobbyId = CSteamID.Nil;
            _nm.ClientManager.StopConnection();
            _isDisconnecting = false;
        }

        public void Disconnect()
        {
            if (_isDisconnecting) return;
            _isDisconnecting = true;

            if (_lobbyId != CSteamID.Nil)
            {
                SteamMatchmaking.LeaveLobby(_lobbyId);
                _lobbyId = CSteamID.Nil;
            }
            if (_nm.IsServerOnlyStarted)
            {
                _nm.ServerManager.StopConnection(true);
            }
            else if (_nm.IsClientOnlyStarted)
            {
                _nm.ClientManager.StopConnection();
            }
            else if (_nm.IsHostStarted)
            {
                _nm.ClientManager.StopConnection();
                _nm.ServerManager.StopConnection(true);
            }

            _isDisconnecting = false;
        }
    }
}