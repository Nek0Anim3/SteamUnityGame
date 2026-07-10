using System;
using System.Threading.Tasks;
using UnityEngine;

public class UINetBus : MonoBehaviour
{
    public static UINetBus Instance;
    public event Action OnHostRequested;
    public event Action OnClientRequested;
    public event Action<string> OnStartRequested;
    public event Action OnDisconnectRequested;

    public void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return;}
        Instance = this;
    }
    
    public void UI_HostGame()
    {
        Debug.Log("UI_HostGame: Calling host!");
        OnHostRequested?.Invoke();
    }

    public void UI_ConnectClient()
    {
        Debug.Log("UI_ConnectClient: Calling connection to the game!");
        OnClientRequested?.Invoke();
    }

    public void UI_Disconnect()
    {
        Debug.Log("UI_Disconnect: Calling disconnection!");
        OnDisconnectRequested?.Invoke();
    }

    public void UI_StartGame()
    {
        Debug.Log("UI_StartGame: Calling start!");
        OnStartRequested?.Invoke("GameScene");
    }

}
