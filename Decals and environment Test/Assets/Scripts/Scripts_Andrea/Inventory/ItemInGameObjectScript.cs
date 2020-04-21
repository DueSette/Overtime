using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField] AudioClip pickupSound; 
    [SerializeField] ItemInventory.Item item;
    [SerializeField] bool destroyOnPickup = false;

    void IInteractable.InteractWith()
    {
        if (inventoriesManager == null)
            inventoriesManager = FindObjectOfType<InventoriesManager>();

        inventoriesManager.itemManager.UnlockNewItem(item);
        inventoriesManager.ForceToggleItemInventoryWindow(true);
        SoundManager.instance.PlaySound(pickupSound);

        OnInteraction();
    }

    protected virtual void OnInteraction()
    {
        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
