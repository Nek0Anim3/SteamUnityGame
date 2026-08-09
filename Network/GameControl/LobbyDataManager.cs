using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Network.GameControl;
using UnityEngine;


public class LobbyDataManager : NetworkBehaviour
{
    
    public static LobbyDataManager Instance;
    private readonly SyncList<LobbyPlayerNames> _playerNameData;
    private List<string> _playerNames;
    
    public event Action<List<string>> OnLobbyEnteredNameData;
    
    //Event
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _playerNames = new List<string>();
    }
    
    //Event
    public override void OnStartServer()
    {
        base.OnStartClient();
        _playerNameData.OnChange += SendUIMessage;
        
        if (IsServerInitialized)
        {
            NetworkManager.ServerManager.OnRemoteConnectionState += OnClientDisconnect;
            RequestClearNicknames();
            _playerNameData.Add(new LobbyPlayerNames{ClientId = NetworkManager.ClientManager.Connection.ClientId, Name = PlayerPrefs.Nickname});
        }
        else
        {
            RequestAddNickname(PlayerPrefs.Nickname, NetworkManager.ClientManager.Connection.ClientId);
        }
        RefreshUI();
    }
    
    //Event
    public override void OnStopServer()
    {
        
        _playerNameData.OnChange -= SendUIMessage;
        if (IsServerStarted)
        {
            NetworkManager.ServerManager.OnRemoteConnectionState -= OnClientDisconnect;
        }
    }
    
    //Event
    private void OnClientDisconnect(NetworkConnection connData, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            int clientId = connData.ClientId;
            LobbyPlayerNames? playerName = FindByClientId(clientId);
            if (playerName.HasValue)
            { 
                _playerNameData.Remove(playerName.Value);    
            }
        }

    }

    private void SendUIMessage(SyncListOperation syncListOperation, int id, LobbyPlayerNames playerNamesOld, LobbyPlayerNames playerNamesNew, bool asServer)
    {
        Debug.Log($"SendUIMessage invoke");
        
        RefreshUI();
    }

    private void RefreshUI()
    {
        Debug.Log($"Lobby name UI sync");
        _playerNames.Clear();
        foreach (var player in _playerNameData)
        {
            _playerNames.Add(player.Name.ToString());
        }
        OnLobbyEnteredNameData?.Invoke(_playerNames);
    }
    
    private LobbyPlayerNames? FindByClientId(int clientId)
    {
        foreach (var entry in _playerNameData)
        {
            if (entry.ClientId == clientId) return entry;
        }
        return null; 
    }
    
    
    public void RequestAddNickname(string name, int clientId)
    {
        
        AddPlayerNameServerRpc(name, clientId);
    }

    public void RequestClearNicknames()
    {
        if (!IsServerStarted) return;
        _playerNameData.Clear();
    }
    
    [ServerRpc]
    private void RemovePlayerNameServerRpc(int clientId)
    {
        LobbyPlayerNames? playerName = FindByClientId(clientId);
        if (playerName == null) return;
        _playerNameData.Remove(playerName.Value);
    }
    
    
    [ServerRpc]
    private void AddPlayerNameServerRpc(string name, int clientId)
    {
        if (FindByClientId(clientId).HasValue) return;
        
        Debug.Log("Added to player name: " + name);
        _playerNameData.Add(new LobbyPlayerNames{ClientId = clientId, Name = name});
        
    }
}
