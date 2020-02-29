using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 *  ======= CLASS SUMMARY =======
 *  This class takes care of storing, displaying and updating the notes inventory, managing both the canvas and the internal data
 */

public class NoteInventory : MonoBehaviour
{
    [System.Serializable]
    public class NoteEntryItem
    {
        public int noteID; //let's say there are 20 notes in the game: this is their actual order in the collection
        public string entryName;
        public string entryDescription;
        public GameObject noteModel;
    }

    public TextMeshProUGUI descriptionUIText;
    public GameObject descriptionVeil; //an almost opaque black panel that only serves the purpose of masking the currently rendered object to make text more readable
    public List<NoteEntryItem> noteList = new List<NoteEntryItem>(); //all the actual info about the notes, set via editor or through pickup items

    [SerializeField] GameObject noteNamePrefab;
    [SerializeField] GameObject noteNameContainer; //we need this to make stuff scrollable
    private List<GameObject> noteUINameGameObjects = new List<GameObject>(); //this is the UI objects with the names of the notes

    [SerializeField] Transform inventoryCameraSpot;
    [SerializeField] Transform modelContainer;
    [SerializeField] GameObject parent;

    int currentFocus = 0;

    void Start()
    {
        InitialiseNoteUIObjects(); //fills a necessary array of references to gameobjects
        InitialiseOwnedNotes3D(); //loads stuff that was set via editor or from save file

        parent.SetActive(false); //initialise everything and then disappear from view
    }

    #region StartUp Functions
    void InitialiseNoteUIObjects() //spawns UI game objects containing the title of each note
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            noteUINameGameObjects.Add(Instantiate(noteNamePrefab, noteNameContainer.transform)); //what is spawned is the TextMeshPro note's name            
            noteUINameGameObjects[i].GetComponent<NoteUIObjectScript>().noteID = noteList[i].noteID;
            noteUINameGameObjects[i].GetComponent<TextMeshProUGUI>().SetText(noteList[i].entryName);
        }
        ScrollEntries(0);
    }

    void InitialiseOwnedNotes3D() //spawns the models for the inventory camera
    {
        //technically, here we would put the function that retrieves the currently collected notes from saved file
        for (int i = 0; i < noteList.Count; i++)
        {
            noteList[i].noteModel = Instantiate(noteList[i].noteModel, Vector3.zero, Quaternion.Euler(0, -90, 0), modelContainer);
            noteList[i].noteModel.transform.localPosition = Vector3.zero;
            noteList[i].noteModel.layer = LayerMask.NameToLayer("ViewableObjects"); //makes sure we are on the layer the inventory cam can see
        }
    }
    
    #endregion

    #region Reoccurring Functions
    public void ScrollEntries(bool up) //if "up" is true then the player is scrolling up the list
    {
        CleanPreviousNoteUI();
        currentFocus += up ? -1 : 1;
        currentFocus = Mathf.Clamp(currentFocus, 0, noteList.Count - 1);
        UpdateNoteDescriptionUI();
    }

    public void ScrollEntries(int index) //put a specific element in focus
    {
        CleanPreviousNoteUI();
        currentFocus = index;
        currentFocus = Mathf.Clamp(currentFocus, 0, noteList.Count - 1);
        UpdateNoteDescriptionUI();
    }

    public void ScrollEntriesToLast()
    {
        CleanPreviousNoteUI();
        UpdateNoteDescriptionUI();
    }

    public void CleanPreviousNoteUI() //clear previously in-focus entry (set color and style back to normal, clear related 3D Object from view
    {
        GameObject currentNoteObject = noteUINameGameObjects[currentFocus]; //this needs to always be the first line of the method

        currentNoteObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        currentNoteObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

        if (noteList[currentFocus].noteModel != null)
            noteList[currentFocus].noteModel.SetActive(false);
    }

    void UpdateNoteDescriptionUI() //updates description, background model, color of the currently selected entry
    {
        NoteEntryItem currentNote = noteList[currentFocus];

        GameObject current3DObj = currentNote.noteModel;
        descriptionUIText.text = currentNote.entryDescription; //update text section

        if (current3DObj != null)
        {
            current3DObj.SetActive(true);
            current3DObj.transform.position = inventoryCameraSpot.position;
        }

        noteUINameGameObjects[currentFocus].GetComponent<TextMeshProUGUI>().color = Color.red;
        noteUINameGameObjects[currentFocus].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        noteUINameGameObjects[currentFocus].GetComponent<Button>().Select();
    }

    public void UnlockNewNote(NoteEntryItem newNote) //call when a player finds a new note
    {
        //MAKING SURE THE ITEM IS NOT ALREADY IN OUR POSSESSION
        
        for (int i = 0; i < noteList.Count; i++)
            if (noteList[i] == newNote) //if the new item is actually an Item that is still on the map but is clicked again
                currentFocus = i;

        foreach (NoteEntryItem i in noteList)  //if we already are in possess of this item don't add it, but still open the inventory       
            if (i == newNote)
                return;

        noteList.Add(newNote);

        //SPAWNS 3 MODEL OF NEW NOTE
        newNote.noteModel = Instantiate(newNote.noteModel, Vector3.zero, Quaternion.Euler(0, -90, 0), modelContainer);
        newNote.noteModel.transform.localPosition = Vector3.zero;
        newNote.noteModel.layer = LayerMask.NameToLayer("ViewableObjects"); //makes sure we are on the layer the inventory cam can see

        //SPAWNS UI OBJECT OF NEW NOTE AND UPDATES IT WITH NOTE'S DATA
        noteUINameGameObjects.Add(Instantiate(noteNamePrefab, noteNameContainer.transform));
        noteUINameGameObjects[noteUINameGameObjects.Count - 1].GetComponent<TextMeshProUGUI>().text = newNote.entryName;

        ScrollEntries(noteList.Count);
    }
    #endregion
}
