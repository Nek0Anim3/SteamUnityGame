using System;
using UnityEngine;

namespace Player
{
    public class LocalData : IPlayerData
    {
        private System.Random _random;
        
        
        //Дебаг конструктор для рандом инстанса
        public LocalData()
        {
            _random = new System.Random();
        }
        
        
        public void SetNickname(string Nickname)
        {
            PlayerPrefs.Nickname = Nickname;
            Debug.Log($"Client Nickname: {PlayerPrefs.Nickname}");
        }

        public void SetUID(ulong clientId)
        {
                                    //Debug random client id
            PlayerPrefs.ClientID = (ulong)_random.Next(1, 10000);  
            Debug.Log($"Client UID: {PlayerPrefs.ClientID}");
        }
    }
}