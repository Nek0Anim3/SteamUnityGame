using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New ItemDatabase", menuName = "Items/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemBase> items = new List<ItemBase>();
    }
}