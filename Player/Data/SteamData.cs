using Steamworks;
using UnityEngine;

namespace Player
{
    public class SteamData : IPlayerData
    {
        public void SetNickname(string nickname)
        {
            if (!SteamClient.IsValid) { return; }
            PlayerPrefs.Nickname = SteamClient.Name;
            Debug.Log($"Client name: {PlayerPrefs.Nickname}");
        }

        public void SetUID(ulong uid)
        {
            if (!SteamClient.IsValid) { return; }
            PlayerPrefs.ClientID = SteamClient.SteamId;
            Debug.Log($"Client UID: {PlayerPrefs.ClientID}");
        }
    }
}