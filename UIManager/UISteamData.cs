using Steamworks;
using TMPro;
using UnityEngine;

public class UISteamData : MonoBehaviour
{
    [SerializeField] private TMP_Text NickName;
    
    void Start()
    {
        NickName.text = $"Welcome, {SteamFriends.GetPersonaName()}";
    }



}
