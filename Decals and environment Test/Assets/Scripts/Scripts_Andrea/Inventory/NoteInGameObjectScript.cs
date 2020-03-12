using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField] NoteInventory.NoteEntryItem note;

    [SerializeField, Tooltip("Note number, ideally they should focus an aenumerated order")] int noteID = -1;
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
