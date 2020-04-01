using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoriesManager : MonoBehaviour
{
    //=== Class summary ===
    //This class simply manages the two inventories (note inventory and item inventory)
    //It contains functions for opening, closing and browsing the inventories

    public bool noteInventoryOpen = false;
    public bool itemInventoryOpen = false;
    public NoteInventory noteManager;
    public ItemInventory itemManager;
    [SerializeField] AudioClip openInventory;
    [SerializeField] AudioClip navigateInventory;

    public static InventoriesManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        itemManager.gameObject.SetActive(true);
        noteManager.transform.parent.gameObject.SetActive(true); //activates the inventory, which will deactivate itself in the same frame. This loads the note inventory.
    }

    void Update()
    {
        if (GameStateManager.gameState == GameState.IN_GAME || GameStateManager.gameState == GameState.MENU)
            CheckInput();
    }

    void CheckInput()
    {
        //Just input stuff
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (itemInventoryOpen) //if the other inventory is open, close it
                ForceToggleItemInventoryWindow(false);

            ToggleNoteInventoryWindow();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (noteInventoryOpen) //if the other inventory is open, close it
                ForceToggleNoteInventoryWindow(false);

            ToggleItemInventoryWindow();
        }

        if (noteInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                noteManager.ScrollEntries(false);
                SoundManager.instance.PlaySound(navigateInventory);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                noteManager.ScrollEntries(true);
                SoundManager.instance.PlaySound(navigateInventory);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                SoundManager.instance.PlaySound(navigateInventory);
                ToggleNotesDescriptionBox();
            }

        }
        else if (itemInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                itemManager.ScrollItems(false);
                SoundManager.instance.PlaySound(navigateInventory);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                itemManager.ScrollItems(true);
                SoundManager.instance.PlaySound(navigateInventory);
            }
        }
    }

    public bool HasItem(string itemName) //checks if we have an item by searching its name 
    {
        foreach (ItemInventory.Item i in itemManager.itemList)
            if (i.name == itemName)
                return true;

        return false;
    }

    //checks if we have an item by searching its name, if it does also removes the item from the list, also returns the item via out keyword
    public bool HasItemAndRemove(string itemName, out GameObject objectReturned)
    {
        foreach (ItemInventory.Item i in itemManager.itemList)
            if (i.name == itemName)
            {
                itemManager.RemoveItem(i);
                objectReturned = i.model;
                return true;
            }

        objectReturned = null;
        return false;
    }

    #region NoteInventory Methods
    void ToggleNoteInventoryWindow() //opens or closes the note inventory, based on its current status
    {
        if (noteInventoryOpen) //if note inventory was open, reset UI-related stuff and close
        {
            noteManager.CleanPreviousNoteUI();
            noteManager.ScrollEntriesToLast();
            GameStateManager.SetGameState(GameState.IN_GAME);
        }
        else //if we opened the menu
            GameStateManager.SetGameState(GameState.MENU);

        SoundManager.instance.PlaySound(openInventory);
        noteInventoryOpen = !noteInventoryOpen;
        noteManager.transform.parent.gameObject.SetActive(noteInventoryOpen);
    }

    public void ForceToggleNoteInventoryWindow(bool openOverride) //instead of toggling from on to off and viceversa, it sets the state
    {
        if (openOverride) //if it was open, reset UI-related stuff and close
        {
            noteManager.CleanPreviousNoteUI();
            noteManager.ScrollEntriesToLast();
        }

        GameStateManager.SetGameState(openOverride ? GameState.MENU : GameState.IN_GAME);
        SoundManager.instance.PlaySound(openInventory);
        noteInventoryOpen = openOverride;
        noteManager.transform.parent.gameObject.SetActive(openOverride);
    }

    //call when we want the menu to open automatically once player collects a note
    public void OpenNoteInventoryWindowOnUnlock(int unlockedNoteID)
    {
        noteManager.CleanPreviousNoteUI();
        noteManager.ScrollEntries(unlockedNoteID);
        GameStateManager.SetGameState(GameState.MENU);
        noteInventoryOpen = true;
        noteManager.transform.parent.gameObject.SetActive(true);
    }

    void ToggleNotesDescriptionBox()
    {
        noteManager.descriptionUIText.enabled = !noteManager.descriptionUIText.enabled;
        noteManager.descriptionVeil.SetActive(noteManager.descriptionUIText.enabled);
    }
    #endregion

    #region ItemInventory methods
    void ToggleItemInventoryWindow()
    {
        itemInventoryOpen = !itemInventoryOpen;

        if (itemInventoryOpen) //if it means that we opened it, then set up item inventory
        {
            itemManager.CleanPreviousUI();
            itemManager.gameObject.SetActive(itemInventoryOpen);
            itemManager.UpdateUI();
            GameStateManager.SetGameState(GameState.MENU);
            SoundManager.instance.PlaySound(openInventory);
        }

        else //if not, clean the UI
        {
            itemManager.CleanPreviousUI();
            itemManager.gameObject.SetActive(itemInventoryOpen);
            GameStateManager.SetGameState(GameState.IN_GAME);
            SoundManager.instance.PlaySound(openInventory);
        }
    }

    public void ForceToggleItemInventoryWindow(bool openOverride)
    {
        itemInventoryOpen = openOverride;

        if (itemInventoryOpen) //if we opened it, then set up item inventory
        {
            itemManager.gameObject.SetActive(itemInventoryOpen);
            itemManager.UpdateUI();
            SoundManager.instance.PlaySound(openInventory);
        }

        else //if not, clean the UI
        {
            itemManager.CleanPreviousUI();
            itemManager.gameObject.SetActive(itemInventoryOpen);
            SoundManager.instance.PlaySound(openInventory);
        }

        GameStateManager.SetGameState(openOverride ? GameState.MENU : GameState.IN_GAME);
    }
    #endregion
}
