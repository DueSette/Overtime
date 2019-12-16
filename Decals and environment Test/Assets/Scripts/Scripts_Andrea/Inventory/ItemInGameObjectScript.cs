using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInGameObjectScript : MonoBehaviour, IInteractable
{
    static InventoriesManager inventory;
    [SerializeField] ItemInventory.Item item;
    [SerializeField] bool destroyOnPickup = false;

    void IInteractable.InteractWith()
    {
        inventory.itemManager.UnlockNewItem(item);
        inventory.ToggleItemInventoryWindow(true);
        if (destroyOnPickup)
            Destroy(gameObject);

        InteractionEvent();
    }

    void Start()
    {
        if (inventory == null)
            inventory = FindObjectOfType<InventoriesManager>();
    }

    protected virtual void InteractionEvent()
    {

    }
}
