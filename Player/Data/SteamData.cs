using Steamworks;
using UnityEngine;

namespace Player
{
    public class SteamData : IPlayerData
    {
        public void SetNickname(string nickname)
        {
            if (!SteamAPI.IsSteamRunning()) { return; }
            PlayerPrefs.Nickname = SteamFriends.GetPersonaName();
            Debug.Log($"Client name: {PlayerPrefs.Nickname}");
        }

        public void SetUID(ulong uid)
        {
            if (!SteamAPI.IsSteamRunning()) { return; }
            PlayerPrefs.ClientID = SteamUser.GetSteamID().m_SteamID;
            Debug.Log($"Client UID: {PlayerPrefs.ClientID}");
        }
    }
}