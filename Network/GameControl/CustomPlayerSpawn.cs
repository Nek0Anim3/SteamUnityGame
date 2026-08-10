using System;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Network.GameControl
{
    public class CustomPlayerSpawn : NetworkBehaviour
    {
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;
        private int _spawnIdx;

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkManager.SceneManager.OnClientLoadedStartScenes += OnClientLoaded;
            
            //Spawn each player that ALREADY connected
            foreach (var clientConn in ServerManager.Clients.Values)
            {
                TrySpawn(clientConn);
            }
        }

        public override void OnStopServer()
        {
            NetworkManager.SceneManager.OnClientLoadedStartScenes -= OnClientLoaded;
            base.OnStopServer();
        }
        
        // Every NEW connection to server triggers
        private void OnClientLoaded(NetworkConnection conn, bool asServer) 
        {
            if (!asServer) return;
            TrySpawn(conn);            
        }

        private void TrySpawn(NetworkConnection conn)
        {
            Transform spawnPoint = spawnPoints[_spawnIdx % spawnPoints.Length];
            _spawnIdx++;
            NetworkObject instance = NetworkManager.GetPooledInstantiated(playerPrefab, asServer: true);
            instance.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            ServerManager.Spawn(instance, conn);
        }
    }
}