using DG.Tweening;
using Player.PlayerMovement;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDAnimationManager : MonoBehaviour
{
    public HUDAnimationManager Instance;
    [SerializeField] private CanvasGroup HUD_SprintGroup;

    public void ShowSprintBar()
    {
        HUD_SprintGroup.DOKill();
        HUD_SprintGroup.DOFade(1.0f, 0.3f);
    }

    public void HideSprintBar()
    {
        HUD_SprintGroup.DOKill();
        HUD_SprintGroup.DOFade(0.0f, 0.3f);
    }
    
}
