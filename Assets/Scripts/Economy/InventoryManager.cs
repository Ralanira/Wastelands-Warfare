using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    public enum ItemType
    {
        Minerals,
        Energium
    }
    public class InventoryManager : MonoBehaviour
    {
        public int maxItemsInInventory = 5;
        public List<ItemType> carriedItems;
    }
}
