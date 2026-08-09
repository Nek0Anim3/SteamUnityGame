using FishNet.Object;
using Player.PlayerMovement;
using UnityEngine;

public class HUDInitializer : NetworkBehaviour
{
    [SerializeField] private GameObject HUDPrefab;
    [SerializeField] private PlayerStamina playerStamina;
    [SerializeField] private HUDItemRaycaster _itemRaycaster;
    private GameObject hudInstance;
    private HUD_Stamina hudStamina;
    private HUDAnimationManager HUDAnimation;

    public override void OnStartClient()
    {
        if (!IsOwner) return;
        hudInstance = Instantiate(HUDPrefab);
        UIItemHint itemHint = hudInstance.GetComponent<UIItemHint>();
        itemHint.Init(_itemRaycaster);
        hudStamina = hudInstance.GetComponent<HUD_Stamina>();
        HUDAnimation = hudInstance.GetComponent<HUDAnimationManager>();
        hudStamina.playerStamina = playerStamina;
        
        playerStamina.OnStaminaChange += hudStamina.ChangeStaminaBar;
        
        //HUDAnimation.Instance
        playerStamina.OnSprintStart += HUDAnimation.ShowSprintBar;
        playerStamina.OnSprintStop += HUDAnimation.HideSprintBar;
    }

    public override void OnStopClient()
    {
        playerStamina.OnSprintStart -= HUDAnimation.ShowSprintBar;
        playerStamina.OnSprintStop -= HUDAnimation.HideSprintBar;
    }
    //methods to get smth from hud
    public UICrosshair GetCrosshair()
    {
        return hudInstance.GetComponent<UICrosshair>();
    }
}
