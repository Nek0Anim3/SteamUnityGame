using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISteamData : MonoBehaviour
{
    [SerializeField] private TMP_Text NickName;
    
    void Start()
    {
        NickName.text = $"Welcome, {SteamClient.Name}";
    }



}
