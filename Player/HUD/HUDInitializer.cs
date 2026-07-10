using Player.PlayerMovement;
using Unity.Netcode;
using UnityEngine;

public class HUDInitializer : NetworkBehaviour
{
    
    [SerializeField] private GameObject HUDPrefab;
    [SerializeField] private PlayerStamina playerStamina;
    private GameObject hudInstance;
    private HUD_Stamina hudStamina;
    private HUDAnimationManager HUDAnimation;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        hudInstance = Instantiate(HUDPrefab);
        hudStamina = hudInstance.GetComponent<HUD_Stamina>();
        HUDAnimation = hudInstance.GetComponent<HUDAnimationManager>();
        hudStamina.playerStamina = playerStamina;
        
        playerStamina.OnStaminaChange += hudStamina.ChangeStaminaBar;
        
        //HUDAnimation.Instance
        playerStamina.OnSprintStart += HUDAnimation.ShowSprintBar;
        playerStamina.OnSprintStop += HUDAnimation.HideSprintBar;
    }

    public override void OnNetworkDespawn()
    {
        playerStamina.OnSprintStart -= HUDAnimation.ShowSprintBar;
        playerStamina.OnSprintStop -= HUDAnimation.HideSprintBar;
    }
}
