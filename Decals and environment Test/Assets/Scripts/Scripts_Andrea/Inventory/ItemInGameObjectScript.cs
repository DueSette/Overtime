using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventory;
    [SerializeField] ItemInventory.Item item;
    [SerializeField] bool destroyOnPickup = false;

    void IInteractable.InteractWith()
    {
        if (inventory == null)
            inventory = FindObjectOfType<InventoriesManager>();

        inventory.itemManager.UnlockNewItem(item);
        inventory.ForceToggleItemInventoryWindow(true);

        InteractionEvent();

        if (destroyOnPickup)
            Destroy(gameObject);
    }

    protected virtual void InteractionEvent()
    {
        
    }
}
