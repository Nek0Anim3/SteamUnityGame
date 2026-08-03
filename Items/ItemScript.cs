using System;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private ItemBase itemData;
    private ValueTuple<string, string, Sprite> itemInfo;
    
    private void Awake()
    {
        itemInfo = (itemData.itemName, itemData.description, itemData.itemIcon);
        
    }

    public ValueTuple<string, string, Sprite> GetItemData()
    {
        return itemInfo;
    }
}
