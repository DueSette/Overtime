using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField] NoteInventory.NoteEntryItem note;

    [SerializeField, Tooltip("Which note does this unlock?")] int noteID = -1;
    [SerializeField] bool destroyOnPickup = false;

    public void InteractWith()
    {
        if (inventoriesManager == null)
            inventoriesManager = FindObjectOfType<InventoriesManager>();

        inventoriesManager.noteManager.UnlockNewNote(note);
        inventoriesManager.OpenNoteInventoryWindowOnUnlock(noteID);

        OnInteraction();
    }

    protected virtual void OnInteraction()
    {
        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
