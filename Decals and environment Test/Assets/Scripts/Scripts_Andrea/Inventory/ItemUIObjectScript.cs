using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIObjectScript : MonoBehaviour
{
    
    public ItemInventory.Item containedItem = null;
    static ItemInventory inventoryManager;

    public void UpdateUI()
    {
        if (inventoryManager == null)
            inventoryManager = GetComponentInParent<ItemInventory>();

        inventoryManager.ScrollItemsFromUI(containedItem.name);
    }

    public void PlaySound(AudioClip clip) //hooked to the onClick button's method
    {
        SoundManager.instance.PlaySound(clip);
    }
}
