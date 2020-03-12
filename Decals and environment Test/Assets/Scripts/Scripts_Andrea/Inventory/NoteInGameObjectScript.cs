using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField] NoteInventory.NoteEntryItem note;

    int noteID;
    [SerializeField] bool destroyOnPickup = false;

    private void Start()
    {
        noteID = note.noteID;
    }

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
