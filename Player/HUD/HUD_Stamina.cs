using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Stamina : MonoBehaviour
{
    public PlayerStamina playerStamina;
    public Slider staminaBar;

    public void ChangeStaminaBar(float newValue)
    {
        staminaBar.value = newValue;
    }
}
