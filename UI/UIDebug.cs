using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIDebug : MonoBehaviour
    {
        public static UIDebug Instance;
        
        [SerializeField] private TMP_Text mouseX;
        [SerializeField] private TMP_Text mouseY;
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return;}

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void UpdateUI(Vector2 mousePos)
        {
            mouseX.text = $"Mouse X: {Math.Round(mousePos.x, 2)}";
            mouseY.text = $"Mouse Y: {Math.Round(mousePos.y, 2)}";
        }  
      
        
    }
}