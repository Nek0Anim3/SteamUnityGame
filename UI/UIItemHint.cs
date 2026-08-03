using System;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHint : MonoBehaviour
{
    private HUDItemRaycaster _itemInfo;
    [SerializeField] private CanvasGroup hudPanel;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Image itemSprite;
    public void Init(HUDItemRaycaster itemInfo)
    {
        _itemInfo = itemInfo;
    }
    
    private void Start()
    {
        hudPanel.alpha = 0.0f;
        _itemInfo.OnItemVisible += Show;
        _itemInfo.OnItemDisappeared += Hide;
    }

    public void Show((string name, string description, Sprite sprite) itemInfo)
    {
        itemName.text = itemInfo.name;
        itemDescription.text = itemInfo.description;
        itemSprite.sprite = itemInfo.sprite;
        hudPanel.DOFade(1.0f, 0.12f);
    }

    public void Hide()
    {
        hudPanel.DOFade(0.0f, 0.12f);
    }
}
