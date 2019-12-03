using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInGameObjectScript : MonoBehaviour, IInteractable
{
    static InventoriesManager inventoriesManager;
    [SerializeField, Tooltip("Which note does this unlock?")] int noteID = -1;

    public void InteractWith()
    {
        inventoriesManager.noteManager.UnlockNote(noteID);
        inventoriesManager.OpenNoteInventoryWindowOnUnlock(noteID);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(inventoriesManager == null)
            inventoriesManager = FindObjectOfType<InventoriesManager>();
    }
}
