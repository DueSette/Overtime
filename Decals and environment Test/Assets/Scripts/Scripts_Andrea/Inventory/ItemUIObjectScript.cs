using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIObjectScript : MonoBehaviour
{
    
    public ItemInventory.Item containedItem = null;

    public void UpdateUI()
    {
        ItemInventory iMan = GetComponentInParent<ItemInventory>();
        iMan.ScrollItemsFromUI(containedItem);
    }
}
