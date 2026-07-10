using System;
using UIManager.MainMenu;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventController : MonoBehaviour
{
    public MenuStates menuPage;
    private Button clickButton;

    private void Awake()
    {
        clickButton = GetComponent<Button>();
    }
    private void Start()
    {
        clickButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenuPage((int)menuPage));
    }
}
