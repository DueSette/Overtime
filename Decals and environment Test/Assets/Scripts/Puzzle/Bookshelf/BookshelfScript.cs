using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfScript : MonoBehaviour, IInteractable
{
    /*===========
     * SELF CONTAINED CLASS FOR FUSEBOX PUZZLE LOGIC
    =============*/
    [SerializeField, Tooltip("When the player interacts, the game searches for the fuses in the inventory using the name of the item(s)")]
    string[] nameOfBooksToSearchInInventory;

    [SerializeField, Tooltip("Each book object that STARTS within the box - do not put inventory books here")]
    List<GameObject> bookPrefabs = new List<GameObject>();

    [SerializeField] AudioClip pickBook, putBookStool, solvedSound;

    ShelfSlotScript[] fuseSlots; //all the places where you can fit a fuse in, they are classes as they contain a package of info

    StoolScript stool;
    AudioSource aud;

    [SerializeField] LayerMask bookshelfLayer;
    [HideInInspector] public GameObject currentlyHeldBook = null; //the fuse the player is currently using

    enum PuzzleState { ACTIVE = 0, PASSIVE = 2, SOLVED = 4 }
    PuzzleState state = PuzzleState.PASSIVE;

    int slotsFilled = 0; //used for keeping track of how many items have been spawned on the tray, internal logic
    private float cameraDist; //will be used as the distance between the player cam and the held fusebox

    #region Unity methods
    private void Start()
    {
        stool = GetComponentInChildren<StoolScript>();
        aud = GetComponent<AudioSource>();

        int i = 0;
        var temp = new List<ShelfSlotScript>();
        temp.AddRange(GetComponentsInChildren<ShelfSlotScript>());

        foreach (ShelfSlotScript f in temp)
        {
            fuseSlots = new ShelfSlotScript[temp.Count];
            fuseSlots[i] = f;
            i++;
        }
    }

    private void Update()
    {
        if (state != PuzzleState.ACTIVE) { return; }

        if (Input.GetButtonUp("Fire1")) { CheckFuseboxInteraction(); }

        if (currentlyHeldBook != null) { UpdateHeldBookPosition(); }
    }

    private void OnEnable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += ExitInteraction;
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= ExitInteraction;
    }
    #endregion

    #region Interaction
    void CheckFuseboxInteraction()
    {
        Ray ray = CameraSwitch.GetCurrentCamera().ScreenPointToRay(Input.mousePosition);
        cameraDist = Vector3.Distance(transform.position, GameStateManager.GetPlayer().transform.position) / 2.1f; //updates the distance from the fusebox to the camera

        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f, bookshelfLayer))
        {
            ShelfSlotScript f = hit.collider.gameObject.GetComponent<ShelfSlotScript>();

            if (f != null) //did we hit a fuseslot?
                f.Interact();

            else if (hit.collider.name.Contains("Stool"))
            {
                if (currentlyHeldBook == null)
                {
                    aud.PlayOneShot(pickBook);
                    stool.HandBook(hit.point);
                }
                else
                {
                    aud.PlayOneShot(putBookStool);
                    stool.StoreBook(currentlyHeldBook);
                }
            }
        }
    }

    void UpdateHeldBookPosition() //makes the fuse follow the cursor on the X and Y, uses a fixed Z distance
    {
        Vector3 v = CameraSwitch.GetCurrentCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist));
        Vector3 vel = Vector3.zero;

        currentlyHeldBook.transform.position = Vector3.SmoothDamp(currentlyHeldBook.transform.position, v, ref vel, 0.025f);
    }

    void ExitInteraction()
    {
        if (state == PuzzleState.PASSIVE) { return; }

        state = PuzzleState.PASSIVE;

        if (currentlyHeldBook != null)
            stool.StoreBook(currentlyHeldBook);

        stool.enabled = false; //prevents mishaps from happening
    }

    public IEnumerator SetSolvedState()
    {
        state = PuzzleState.SOLVED;

        aud.PlayOneShot(solvedSound);

        yield return new WaitForSeconds(1.75f);

        GameStateManager.SetGameState(GameState.IN_GAME);
    }
    #endregion

    #region Startup Methods
    void IInteractable.InteractWith()
    {
        if (state == (PuzzleState.ACTIVE | PuzzleState.SOLVED)) { return; }

        state = PuzzleState.ACTIVE;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the bookshelf in focus

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        stool.enabled = true;
        LoadAvailableBooks();
    }

    public void LoadAvailableBooks() //spawns and displays the starting books in the stool
    {
        PollInventoryForBooks();

        for (; slotsFilled < bookPrefabs.Count; slotsFilled++)
            if (bookPrefabs[slotsFilled] != null)
            {
                GameObject g = Instantiate(bookPrefabs[slotsFilled], stool.slots[slotsFilled]);
                stool.AssignFilledSlot(g);
            }
    }

    private void PollInventoryForBooks() //checks if the inventory has books that are used within this puzzle
    {
        GameObject newBook = null;

        foreach (string s in nameOfBooksToSearchInInventory)
            if (InventoriesManager.instance.HasItemAndRemove(s, out newBook))
            {
                newBook.transform.rotation = Quaternion.identity;
                bookPrefabs.Add(newBook);
            }
    }
    #endregion
}
