using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using Network;

using UnityEngine;
using UnityEngine.SceneManagement;

public class NetSceneControl : NetworkBehaviour
{
    public static NetSceneControl Instance;

    [SerializeField] private GameObject playerPrefab; 
    [SerializeField] private NetworkRoot _root;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject); 
        Instance = this;
    }

    public override void OnStartServer()
    {
        if (IsServerInitialized)
        {
            _root.NetworkManager.SceneManager.OnLoadEnd += OnSceneLoadComplete;
            _root.UI.OnStartRequested += ChangeScene;
        }
    }
    public override void OnStopServer()
    {
        if (IsServerInitialized && _root.NetworkManager != null && _root.NetworkManager.SceneManager != null)
        {
            _root.NetworkManager.SceneManager.OnLoadEnd -= OnSceneLoadComplete;
        }
    }
    
    public void ChangeScene(string sceneName)
    {
        if (!IsServer) return;
        SceneLoadData sceneLoadData = new SceneLoadData(sceneName);
        _root.NetworkManager.SceneManager.LoadGlobalScenes(sceneLoadData);
    }

    private void OnSceneLoadComplete(SceneLoadEndEventArgs args)
    {
        //==============================
        // DEPRECATED
        //==================================
        // FISHNET WORKS DIFFERENT
        // OLD SPAWN LOGIC FOR PLAYERS
        //=================================
        
        /*GameObject[] spawns =  GameObject.FindGameObjectsWithTag("Spawnpoint");
        
        int spawnIndex = 0;
        Debug.Log($"[SceneLoad] Completed: {clientsCompleted.Count}, Timeout: {clientsTimeout.Count}");
        foreach (var id in clientsTimeout)
            Debug.LogWarning($"[SceneLoad] Client {id} TIMED OUT");
        foreach (ulong clientId in clientsCompleted)
        {
            
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;

            if (spawns.Length > 0)
            {
                Transform targetPoint = spawns[spawnIndex % spawns.Length].transform;
                spawnPosition = targetPoint.position;
                spawnRotation = targetPoint.rotation;
                spawnIndex++;
            }
            
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
            netObj.SpawnAsPlayerObject(clientId, true); 
        }*/
        
    }
}
