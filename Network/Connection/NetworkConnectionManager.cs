using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Network
{
    public class NetworkConnectionManager : MonoBehaviour
    {
        private NetworkManager _networkManager;
        private void Start()
        {
            if (_networkManager != null)
            {
                _networkManager.ServerManager.OnRemoteConnectionState += OnNetworkConnect;
            }
        }

        private void OnNetworkConnect(NetworkConnection connData, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
            {
                Debug.Log("[+] Client Connected!");
            }
        }
    }
}