using System;
using System.Collections.Generic;
using Network.GameControl;
using Unity.Netcode;
using UnityEngine;


public class LobbyDataManager : NetworkBehaviour
{
    
    public static LobbyDataManager Instance;
    private NetworkList<LobbyPlayerNames> _playerNameData;
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
        _playerNameData = new NetworkList<LobbyPlayerNames>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        _playerNames = new List<string>();
    }
    
    //Event
    public override void OnNetworkSpawn()
    {
        _playerNameData.OnListChanged += SendUIMessage;
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            RequestClearNicknames();
            _playerNameData.Add(new LobbyPlayerNames{ClientId = NetworkManager.Singleton.LocalClientId, Name = PlayerPrefs.Nickname});
        }
        else
        {
            RequestAddNickname(PlayerPrefs.Nickname, NetworkManager.Singleton.LocalClientId);
        }
        RefreshUI();
    }
    
    //Event
    public override void OnNetworkDespawn()
    {
        
        _playerNameData.OnListChanged -= SendUIMessage;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }
    
    //Event
    private void OnClientDisconnect(ulong clientId)
    {
        LobbyPlayerNames? playerName = FindByClientId(clientId);
        if (playerName.HasValue)
        { 
            _playerNameData.Remove(playerName.Value);    
        }
    }

    private void SendUIMessage(NetworkListEvent<LobbyPlayerNames> e)
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
    
    private LobbyPlayerNames? FindByClientId(ulong clientId)
    {
        foreach (var entry in _playerNameData)
        {
            if (entry.ClientId == clientId) return entry;
        }
        return null; 
    }
    
    
    public void RequestAddNickname(string name, ulong clientId)
    {
        
        AddPlayerNameServerRpc(name, clientId);
    }

    public void RequestRemoveNickname(ulong clientId)
    {
        Debug.Log($"RequestRemoveNickname");
        RemovePlayerNameServerRpc(clientId);
    }

    public void RequestClearNicknames()
    {
        if (!IsServer) return;
        _playerNameData.Clear();
    }
    
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void RemovePlayerNameServerRpc(ulong clientId)
    {
        LobbyPlayerNames? playerName = FindByClientId(clientId);
        if (playerName == null) return;
        _playerNameData.Remove(playerName.Value);
    }
    
    
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void AddPlayerNameServerRpc(string name, ulong clientId)
    {
        if (FindByClientId(clientId).HasValue) return;
        
        Debug.Log("Added to player name: " + name);
        _playerNameData.Add(new LobbyPlayerNames{ClientId = clientId, Name = name});
        
    }
}
