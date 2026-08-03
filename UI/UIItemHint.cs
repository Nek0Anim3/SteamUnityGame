using System;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHint : MonoBehaviour
{
    private HUDItemRaycaster _itemInfo;
    private Vector2 itemPos;
    private Camera pCam;
    
    [SerializeField] private CanvasGroup hudPanel;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Image itemSprite;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform itemTransform;
    public void Init(HUDItemRaycaster itemInfo)
    {
        _itemInfo = itemInfo;
    }
    
    private void Start()
    {
        hudPanel.alpha = 0.0f;
        _itemInfo.OnItemVisible += Show;
        _itemInfo.OnItemDisappeared += Hide;
        /*_itemInfo.OnItemPositionGet += SetPosition;*/
    }

    private void OnDestroy()
    {
        _itemInfo.OnItemVisible -= Show;
        _itemInfo.OnItemDisappeared -= Hide;
        /*_itemInfo.OnItemPositionGet -= SetPosition;*/
    }

    /*private void SetPosition(Vector3 position, Camera cam)
    {
        pCam = cam;
        itemPos = position;
    }

    private void LateUpdate()
    {
        if (hudPanel.alpha == 0.0f) return;
        if (pCam == null) return;
        Vector3 localScreenPos = pCam.WorldToScreenPoint(itemPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, localScreenPos, null, out Vector2 localPoint);
        itemTransform.anchoredPosition = localPoint;
    }*/
    
    private void Show((string name, string description, Sprite sprite) itemInfo)
    {
        itemName.text = itemInfo.name;
        itemDescription.text = itemInfo.description;
        itemSprite.sprite = itemInfo.sprite;
        hudPanel.DOFade(1.0f, 0.12f);
    }
    
    private void Hide()
    {
        hudPanel.DOFade(0.0f, 0.12f);
    }
}
