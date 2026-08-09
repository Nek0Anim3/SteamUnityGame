using System;
using Unity.Netcode;
using UnityEngine;

public class ItemScript : NetworkBehaviour
{
    [SerializeField] private ItemBase itemData;
    private ValueTuple<string, string, Sprite> itemInfo;
    [SerializeField] private Transform UIAnchor;
    private void Awake()
    {
        itemInfo = (itemData.itemName, itemData.description, itemData.itemIcon);
        
    }

    public ValueTuple<string, string, Sprite> GetItemData()
    {
        return itemInfo;
    }

    public Transform GetUIAnchor()
    {
        return UIAnchor;
    }
}
