using Network;
using Steamworks;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    public static GameBootstrap Instance { get; private set; }
    public INetConnectionService Connection { get; private set; }

    [SerializeField] private NetworkRoot networkRoot;
    [SerializeField] private NetworkMode mode; // DirectIp / Steam
    [SerializeField] private string nickname;
    
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.Nickname = nickname;
        Connection = ConnectionServiceFactory.Create(mode, networkRoot);
    }
    
    private void Update()
    {
        (Connection as INetPolling)?.Tick();
    }

    private void OnApplicationQuit()
    {
        Connection.Disconnect();
        if (SteamAPI.IsSteamRunning()) SteamAPI.Shutdown();
    }
}