using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemBase")]
public class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string description;
    public int itemID;
}
