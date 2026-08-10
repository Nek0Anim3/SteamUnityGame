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
        base.OnStartServer();
        if (IsServerInitialized)
        {
            
            _root.UI.OnStartRequested += ChangeScene;
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!IsServerInitialized) return;
        SceneLoadData sceneLoadData = new SceneLoadData(sceneName);
        sceneLoadData.ReplaceScenes = ReplaceOption.All; // Otherwise it adds scene on top of another
        _root.NetworkManager.SceneManager.LoadGlobalScenes(sceneLoadData);
    }


}
