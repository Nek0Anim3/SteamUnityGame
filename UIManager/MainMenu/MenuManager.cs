using System;
using UnityEngine;

namespace UIManager.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
        }
        
        public event Action<int> OnMenuStateChange;

        public void ChangeMenuPage(int invokedPage)
        {
            // Debug.Log($"Changing menu to {newState}");
            OnMenuStateChange?.Invoke(invokedPage);
        }
    }
}
