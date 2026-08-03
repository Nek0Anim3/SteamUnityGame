using System;
using UnityEngine;

public class HUDItemRaycaster : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    private float _rayDistance = 1.7f;
    [SerializeField] private LayerMask interactableLayer;
    private RaycastHit[] _hitResults = new RaycastHit[1];
    
    public event Action<(string, string, Sprite)> OnItemVisible;
    public event Action OnItemDisappeared;
    private bool isVisible;
    void LateUpdate()
    {
        
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.red);
        if (Physics.SphereCastNonAlloc(ray, 0.18f, _hitResults, _rayDistance, interactableLayer, QueryTriggerInteraction.Collide) > 0)
        {
            if (isVisible) return;
            OnItemVisible?.Invoke(_hitResults[0].collider.GetComponent<ItemScript>().GetItemData());

            isVisible = true;
        }
        else
        {
            if (!isVisible) return;
            OnItemDisappeared?.Invoke();
            isVisible = false;
        }
    }
}
