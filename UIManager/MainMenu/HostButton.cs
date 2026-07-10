using UnityEngine;
using UnityEngine.UI;

namespace UIManager.MainMenu
{
    public class HostButton : MonoBehaviour
    {
        private Button hostButton;

        private void Awake()
        {
            hostButton = GetComponent<Button>();
        }

        private void Start()
        {
            
        }
        
    }
}