using System;
using GameManagement;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

namespace Network
{
    public class NetworkConnectionManager : MonoBehaviour
    {
        private void Start()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnConnectionEvent += OnNetworkConnect;
            }
        }

        private void OnNetworkConnect(NetworkManager sender, ConnectionEventData connData)
        {
            switch (connData.EventType)
            {
                case ConnectionEvent.ClientConnected:
                    Debug.Log($"[+]: Client {connData.ClientId} connected!");
                    break;
                case ConnectionEvent.ClientDisconnected:
                    Debug.Log($"[-]: Client {connData.ClientId} disconnected!");
                    break;
                default:
                    Debug.Log("connection event!");
                    break;
            }
        }
    }
}