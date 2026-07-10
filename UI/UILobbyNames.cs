using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILobbyNames : MonoBehaviour
{
    public static UILobbyNames Instance;
    [SerializeField] private TMP_Text playerListText;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    private void Start()
    {
        LobbyDataManager.Instance.OnLobbyEnteredNameData += UpdatePlayerUI;
    }
    
    private void OnDisable()
    {
        if (LobbyDataManager.Instance != null)
        {
            LobbyDataManager.Instance.OnLobbyEnteredNameData -= UpdatePlayerUI;
        }
    }

    private void UpdatePlayerUI(List<string> players)
    {
        playerListText.text = String.Empty;
        playerListText.text = String.Join("\n", players);
    }
}
