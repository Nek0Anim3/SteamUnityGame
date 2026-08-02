using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIDebug : MonoBehaviour
    {
        public static UIDebug Instance;

        [SerializeField] private TMP_Text distance;
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return;}

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void UpdateUI(float distance)
        {
            this.distance.text = $"Distance to player: {distance}";
        }  
      
        
    }
}