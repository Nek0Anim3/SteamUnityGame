using System.Collections.Generic;
using Network;
using Unity.Netcode;
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
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _root.NetworkManager.SceneManager.OnLoadEventCompleted += OnSceneLoadComplete;
            _root.UI.OnStartRequested += ChangeScene;
        }
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer && _root.NetworkManager != null && _root.NetworkManager.SceneManager != null)
        {
            _root.NetworkManager.SceneManager.OnLoadEventCompleted -= OnSceneLoadComplete;
        }
    }
    
    public void ChangeScene(string sceneName)
    {
        if (!IsServer) return;
        
        SceneEventProgressStatus status = _root.NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogError($"Failed to load scene: {status}");
        }
    }

    private void OnSceneLoadComplete(string str, LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimeout)
    {
        GameObject[] spawns =  GameObject.FindGameObjectsWithTag("Spawnpoint");

        int spawnIndex = 0;
        
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
        }
        
    }
}
