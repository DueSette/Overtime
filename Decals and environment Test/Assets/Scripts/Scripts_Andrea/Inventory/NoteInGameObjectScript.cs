using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInGameObjectScript : InGameObjectBaseClass, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] NoteInventory.NoteEntryItem note;

    int noteID;
    [SerializeField] bool destroyOnPickup;

    private void Start()
    {
        noteID = note.noteID;
    }

    public void InteractWith()
    {
        if (inventoriesManager == null)
            inventoriesManager = FindObjectOfType<InventoriesManager>();

        SoundManager.instance.PlaySound(pickupSound);
        inventoriesManager.noteManager.UnlockNewNote(note);
        inventoriesManager.OpenNoteInventoryWindowOnUnlock(noteID);

        OnInteraction();
    }

    protected virtual void OnInteraction()
    {
        if (destroyOnPickup)
            gameObject.SetActive(false);
    }
}
