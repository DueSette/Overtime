using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 *  ======= CLASS SUMMARY =======
 *  This class takes care of storing, displaying and updating the notes inventory, managing both the canvas and the internal data
 */
public class NoteInventory : MonoBehaviour
{
    [System.Serializable] class NoteEntryItem
    {
        public int noteID; //let's say there are 20 notes in the game: this is their actual order in the collection
        public bool collected;
        public string entryName;
        public string entryDescription;
        public GameObject noteModel;

        public void Check() //why does this need to be a method? Nobody knows but here we are 
        {
            collected = true;
        }
    }

    public TextMeshProUGUI descriptionUIText;
    [SerializeField] List<NoteEntryItem> noteList = new List<NoteEntryItem>(); //all the actual info about the notes, set via editor

    [SerializeField] GameObject noteNamePrefab;
    [SerializeField] GameObject noteNameContainer; //we need this to make stuff scrollable
    private GameObject[] noteNameGameObjects; //this is the UI objects with the names of the notes

    [SerializeField] Transform inventoryCameraSpot;
    [SerializeField] Transform modelContainer;
    [SerializeField] GameObject parent;

    int currentFocus;
    List<int> notesOwned = new List<int>();

    void Start()
    {
        SpawnNoteUIObject(); //fills a necessary array of references to gameobjects
        InitialiaseOwnedNotes(); //check what entries the player has found
        Spawn3DModels();

        LoadAllEntries();
        ScrollEntries(0);

        parent.SetActive(false); //initialise everything and then disappear from view
    }

    #region StartUp Functions
    void SpawnNoteUIObject() //spawns UI game objects containing the title of each note
    {
        noteNameGameObjects = new GameObject[noteList.Count];
        for (int i = 0; i < noteList.Count; i++)
        {
            noteNameGameObjects[i] = Instantiate(noteNamePrefab, noteNameContainer.transform); //what is spawned is the TextMeshPro note's name            
            noteNameGameObjects[i].GetComponent<NoteUIObjectScript>().noteID = noteList[i].noteID;
        }
    }

    void Spawn3DModels()
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            noteList[i].noteModel = Instantiate(noteList[i].noteModel, Vector3.zero, Quaternion.identity, modelContainer);
            noteList[i].noteModel.transform.localPosition = Vector3.zero;
            noteList[i].noteModel.layer = LayerMask.NameToLayer("ViewableObjects"); //makes sure we are on the layer the inventory cam can see
        }
    }

    void InitialiaseOwnedNotes() //checks what notes we have collected from savefile and gives them their ID
    {
        //This is an example of how we add notes when we first load the level (it unlocks all the notes on game startup)
        //notesOwned.Add(0);
        //notesOwned.Add(3);
        //notesOwned.Add(4);
        //temporary code

        //load from json
    }

    public void LoadAllEntries() //load all gathered notes and sort them by noteID. Possibly read from json save data and retrieve the ID of owned notes
    {
        MarkOwnedNotes();
        ActivateOwnedNotes();

        //SET ALL COLLECTED NOTES (READ FROM MEMBER ID INT ARRAY) TO "COLLECTED"
        void MarkOwnedNotes()
        {
            for (int i = 0; i < notesOwned.Count; i++)
            {
                if (notesOwned[i] > -1)
                {
                    int actID = notesOwned[i];
                    noteList[actID].Check();
                }
            }
        }

        void ActivateOwnedNotes()
        {
            for (int i = 0; i < noteList.Count; i++)
            {
                //among the collected notes, make the appropriate entry on the screen display its name, for un-owned entries, put question marks
                NoteEntryItem note = noteList[i];
                if (noteList[i].collected)
                    noteNameGameObjects[note.noteID].GetComponent<TextMeshProUGUI>().text = noteList[i].entryName;
                else
                    noteNameGameObjects[note.noteID].GetComponent<TextMeshProUGUI>().text = "???";
            }
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
        GameObject currentNoteObject = noteNameGameObjects[currentFocus];

        currentNoteObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        currentNoteObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

        if (noteList[currentFocus].noteModel != null)
            noteList[currentFocus].noteModel.SetActive(false);
    }

    void UpdateNoteDescriptionUI() //updates description, background model, color of the currently selected entry
    {
        NoteEntryItem currentNote = noteList[currentFocus];
        GameObject current3DObj = currentNote.noteModel;
        descriptionUIText.text = currentNote.collected ? currentNote.entryDescription : "???"; //update text section

        if (current3DObj != null && currentNote.collected)
        {
            current3DObj.SetActive(true);
            current3DObj.transform.position = inventoryCameraSpot.position;
        }

        noteNameGameObjects[currentFocus].GetComponent<TextMeshProUGUI>().color = Color.red;
        noteNameGameObjects[currentFocus].GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }

    public void UnlockNote(int noteID) //call when a player finds a new note
    {
        if (noteID < 0)
            return;

        CleanPreviousNoteUI();
        currentFocus = (noteID);

        notesOwned.Add(noteID);
        LoadAllEntries();
        ScrollEntries(noteID);
    }
    #endregion
}
