using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class InventoriesManager : MonoBehaviour
{
    //=== Class summary ===
    //This class simply manages the two inventories (note inventory and item inventory)
    //It contains functions for opening, closing and browsing the inventories

    public NoteInventory noteManager;
    public ItemInventory itemManager;

    [SerializeField] GameObject mainPanel;

    [SerializeField] VolumeProfile pauseMenuPostProcess;

    [SerializeField] Image notesTitle, itemsTitle;
    [SerializeField, Tooltip("If this is unpopulated just call Andrea, easier done than said")] Sprite selectedNotesImage, selectedItemsImage;
    Sprite unselectedNotesImage, unselectedItemsImage;

    [SerializeField] AudioClip openInventory, closeInventory;
    [SerializeField] AudioClip navigateInventory;

    public static InventoriesManager instance;

    void Start()
    {
        if (instance == null)
            instance = this;

        SetMainPanel(true);
        unselectedNotesImage = notesTitle.sprite;
        unselectedItemsImage = itemsTitle.sprite;

        itemManager.InitialiseOwnedItems();
        noteManager.Initialise();
        itemManager.gameObject.SetActive(false);
        ManageTitlesSprites();
        SetMainPanel(false);
    }

    void Update()
    {
        if (GameStateManager.gameState == GameState.IN_GAME || GameStateManager.gameState == GameState.MENU)
            CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)) //OPEN OR CLOSE MENU
            SetMainPanel(!mainPanel.activeSelf);

        if (mainPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ForceToggleNoteInventoryWindow(true);
                ForceToggleItemInventoryWindow(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ForceToggleNoteInventoryWindow(false);
                ForceToggleItemInventoryWindow(true);
            }
            
            if (noteManager.transform.parent.gameObject.activeInHierarchy) //NAVIGATE NOTE INVENTORY
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.S))
                {
                    noteManager.ScrollEntries(false);
                    SoundManager.instance.PlaySound(navigateInventory);
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.W))
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

            else if (itemManager.gameObject.activeInHierarchy) //NAVIGATE ITEM INVENTORY
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.S))
                {
                    itemManager.ScrollItems(false);
                    SoundManager.instance.PlaySound(navigateInventory);
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.W))
                {
                    itemManager.ScrollItems(true);
                    SoundManager.instance.PlaySound(navigateInventory);
                }
            }
        }
    }
    //opens/Closes the UI obejct that holds the inventories
    public void SetMainPanel(bool open)
    {
        mainPanel.SetActive(open);

        SoundManager.instance.PlaySound(open ? openInventory : closeInventory);

        if (open)
            SceneSettingsManager.instance.ChangeToPause(pauseMenuPostProcess);
        else
            SceneSettingsManager.instance.RevertToPreviousProfile();

        GameStateManager.SetGameState(open ? GameState.MENU : GameState.IN_GAME);
    }

    //opens/closes individual inventories in such a way that only one can be open and one closed
    public void OpenInventoriesMutuallyExclusively(bool noteInventoryOpen)
    {
        noteManager.transform.parent.gameObject.SetActive(noteInventoryOpen);
        itemManager.gameObject.SetActive(!noteInventoryOpen);  
    }

    //checks and sets which title on the top part of the menu to highlight
    private void ManageTitlesSprites()
    {
        notesTitle.sprite = noteManager.transform.parent.gameObject.activeInHierarchy ? selectedNotesImage : unselectedNotesImage;
        itemsTitle.sprite = itemManager.gameObject.activeInHierarchy ? selectedItemsImage : unselectedItemsImage;
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
        if (noteManager.transform.parent.gameObject.activeInHierarchy) //if note inventory was open, reset UI-related stuff and close
        {
            noteManager.CleanPreviousNoteUI();
            noteManager.ScrollEntriesToLast();
            GameStateManager.SetGameState(GameState.IN_GAME);
        }
        else //if we opened the menu
            GameStateManager.SetGameState(GameState.MENU);

        SoundManager.instance.PlaySound(openInventory);
        noteManager.transform.parent.gameObject.SetActive(!noteManager.transform.parent.gameObject.activeInHierarchy);
        ManageTitlesSprites();
    }

    public void ForceToggleNoteInventoryWindow(bool openOverride) //instead of toggling from on to off and viceversa, it sets the state
    {
        noteManager.transform.parent.gameObject.SetActive(openOverride);

        if (openOverride) //if it was open, reset UI-related stuff and close
        {
            noteManager.CleanPreviousNoteUI();
            noteManager.ScrollEntriesToLast();

            itemManager.gameObject.SetActive(false);
        }

        SoundManager.instance.PlaySound(openInventory);
        //GameStateManager.SetGameState(openOverride ? GameState.MENU : GameState.IN_GAME);
        ManageTitlesSprites();
    }

    //call when we want the menu to open automatically once player collects a note
    public void OpenNoteInventoryWindowOnUnlock(int unlockedNoteID)
    {
        noteManager.CleanPreviousNoteUI();
        noteManager.ScrollEntries(unlockedNoteID);
        GameStateManager.SetGameState(GameState.MENU);
        noteManager.transform.parent.gameObject.SetActive(true);
        itemManager.gameObject.SetActive(false);
        ManageTitlesSprites();
    }

    void ToggleNotesDescriptionBox()
    {
        noteManager.descriptionUIText.enabled = !noteManager.descriptionUIText.enabled;
        noteManager.spotlight.SetActive(!noteManager.descriptionUIText.enabled);
    }
    #endregion

    #region ItemInventory methods
    void ToggleItemInventoryWindow()
    {
        itemManager.gameObject.SetActive(!itemManager.gameObject.activeInHierarchy);

        if (itemManager.gameObject.activeInHierarchy) //if it means that we opened it, then set up item inventory
        {
            itemManager.CleanPreviousUI();
            itemManager.UpdateUI();
            GameStateManager.SetGameState(GameState.MENU);
            SoundManager.instance.PlaySound(openInventory);
        }

        else //if not, clean the UI
        {
            itemManager.CleanPreviousUI();
            GameStateManager.SetGameState(GameState.IN_GAME);
            SoundManager.instance.PlaySound(openInventory);
        }
        ManageTitlesSprites();
    }

    public void ForceToggleItemInventoryWindow(bool openOverride)
    {
        itemManager.gameObject.SetActive(openOverride);
        if (openOverride)
            noteManager.transform.parent.gameObject.SetActive(false);

        if (itemManager.gameObject.activeInHierarchy) //if we opened it, then set up item inventory
        {
            itemManager.UpdateUI();
            SoundManager.instance.PlaySound(openInventory);
        }

        else //if not, clean the UI
        {
            itemManager.CleanPreviousUI();
            SoundManager.instance.PlaySound(openInventory);
        }

        //GameStateManager.SetGameState(openOverride ? GameState.MENU : GameState.IN_GAME);
        ManageTitlesSprites();
    }
    #endregion
}
