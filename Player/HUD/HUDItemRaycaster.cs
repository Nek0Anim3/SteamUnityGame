using System;
using UnityEngine;

public class HUDItemRaycaster : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    private float _rayDistance = 1.7f;
    [SerializeField] private LayerMask interactableLayer;
    private RaycastHit[] _hitResults = new RaycastHit[5];
    
    public event Action<(string, string, Sprite)> OnItemVisible;
    public event Action<Vector3, Camera> OnItemPositionGet;
    public event Action OnItemDisappeared;
    private bool isVisible;
    private RaycastHit _currentObject;
    private RaycastHit _nearestObject;
    
    void LateUpdate()
    {
        
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.red);
        int hitCount = Physics.SphereCastNonAlloc(ray, 0.18f, _hitResults, _rayDistance, interactableLayer, QueryTriggerInteraction.Collide);
        if (hitCount > 0)
        {
            _nearestObject = _hitResults[0];
            for (int i = 0; i < hitCount; i++)
            {
                if (_hitResults[i].distance < _nearestObject.distance)
                {
                    _nearestObject = _hitResults[i];
                }
            }
            if (_currentObject.collider != null && _currentObject.collider == _nearestObject.collider && isVisible) return;
            ItemScript itemScript = _nearestObject.collider.GetComponent<ItemScript>();
            OnItemVisible?.Invoke(itemScript.GetItemData());
            _currentObject = _nearestObject;
            /*OnItemPositionGet?.Invoke(itemScript.GetUIAnchor().position, playerCam);*/
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
