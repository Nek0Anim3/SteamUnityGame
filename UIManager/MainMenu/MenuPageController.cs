using UnityEngine;

namespace UIManager.MainMenu
{
    public class MenuPageController : MonoBehaviour
    {
        [Header("Menu Type Settings")]
        public MenuStates MenuType;
        [SerializeField] private MenuManager MenuManager;
        public bool isDefault;
        
        [Header("Objects to change")] [SerializeField]
        private GameObject pageElements;
        private void Awake()
        {
            MenuManager.OnMenuStateChange += ChangeToMenu;
        }

        private void Start()
        {
            if (isDefault == false)
            {
                pageElements.SetActive(false);
            }
        }

        public void ChangeToMenu(int page)
        {
            MenuStates state = (MenuStates)page;
            if (state == MenuType)
            {
                pageElements.SetActive(true); 
                return;
            }
            pageElements.SetActive(false);
        }

    }
}
