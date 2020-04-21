using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxScript : MonoBehaviour, IInteractable, ITextPrompt
{
    /*===========
     * SELF CONTAINED CLASS FOR FUSEBOX PUZZLE LOGIC
    =============*/
    [SerializeField, Tooltip("When the player interacts, the game searches for the fuses in the inventory using the name of the item(s)")]
    string[] nameOfFusesToSearchInInventory;

    [SerializeField, Tooltip("Each fuse object that STARTS within the box - do not put inventory fuses here")]
    List<GameObject> fusePrefabs = new List<GameObject>();

    [SerializeField] AudioClip idleSolvedSound, doorOpen, doorClose, pickFuse, putFuseTray, solvedSound, unsolvedSound;

    FuseSlotScript[] fuseSlots; //all the places where you can fit a fuse in, they are classes as they contain a package of info
    
    FuseTrayScript tray;
    Animator anim;
    [SerializeField] AudioSource aud;

    [SerializeField] LayerMask fuseBoxLayer;
    [HideInInspector] public GameObject currentlyHeldFuse = null; //the fuse the player is currently using

    [SerializeField] MeshRenderer mainShell, hubShell, lightningBolt;
    [SerializeField] List<MeshRenderer> ledIndicators = new List<MeshRenderer>();
    [SerializeField] Material correctSolvedMaterial;
    private List<Material> mainShellMaterials = new List<Material>();

    enum PuzzleState { ACTIVE = 0, PASSIVE = 2, SOLVED = 4 }
    PuzzleState state = PuzzleState.PASSIVE;

    int slotsFilled = 0; //used for keeping track of how many items have been spawned on the tray, internal logic
    private float cameraDist; //will be used as the distance between the player cam and the held fusebox

    #region Unity methods
    private void Start()
    {
        tray = GetComponentInChildren<FuseTrayScript>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        int i = 0;
        var temp = new List<FuseSlotScript>();
        temp.AddRange(GetComponentsInChildren<FuseSlotScript>());

        foreach (FuseSlotScript f in temp)
        {
            fuseSlots = new FuseSlotScript[temp.Count];
            fuseSlots[i] = f;
            i++;
        }

        mainShell.GetMaterials(mainShellMaterials);
    }

    private void Update()
    {
        if (state != PuzzleState.ACTIVE) { return; }

        if (Input.GetButtonUp("Fire1")) { CheckFuseboxInteraction(); }

        if (currentlyHeldFuse != null) { UpdateHeldFusePosition(); }
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

        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f, fuseBoxLayer))
        {
            FuseSlotScript f = hit.collider.gameObject.GetComponent<FuseSlotScript>();

            if (f != null) //did we hit a fuseslot?
                f.Interact();

            else if(hit.collider.name.Contains("Tray"))
            {
                if (currentlyHeldFuse == null)
                {
                    aud.PlayOneShot(pickFuse);
                    tray.HandFuse(hit.point);
                }
                else
                {
                    aud.PlayOneShot(putFuseTray);
                    tray.StoreFuse(currentlyHeldFuse);
                }
            }
        }
    }

    void UpdateHeldFusePosition() //makes the fuse follow the cursor on the X and Y, uses a fixed Z distance
    {
        Vector3 v = CameraSwitch.GetCurrentCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist));
        Vector3 vel = Vector3.zero;

        currentlyHeldFuse.transform.position = Vector3.SmoothDamp(currentlyHeldFuse.transform.position, v, ref vel, 0.025f);
    }

    void ExitInteraction()
    {
        if(state == PuzzleState.PASSIVE) { return; }

        state = PuzzleState.PASSIVE;

        anim.SetTrigger("Close");
        StartCoroutine(DelaySound(doorClose));

        if (currentlyHeldFuse != null)
            tray.StoreFuse(currentlyHeldFuse);

        tray.enabled = false; //prevents mishaps from happening
    }

    public IEnumerator SetSolvedState()
    {
        StartCoroutine(DelaySound(idleSolvedSound));

        state = PuzzleState.SOLVED;

        ManageColouredStrips(true);
        TurnOnLightsOnSolved();

        aud.PlayOneShot(solvedSound);

        yield return new WaitForSeconds(1.75f);
        anim.SetTrigger("Close");
        StartCoroutine(DelaySound(doorClose));

        GameStateManager.SetGameState(GameState.IN_GAME);

        // Triggering Next Level Event
        LevelManager.onLevelEvent("FuseBoxPuzzleSolved");
    }
    #endregion

    #region Startup Methods
    void IInteractable.InteractWith()
    {
        if(state == (PuzzleState.ACTIVE | PuzzleState.SOLVED))
        {
            return;
        }

        state = PuzzleState.ACTIVE;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the fusebox in focus
        anim.SetTrigger("Open");
        aud.PlayOneShot(doorOpen);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        tray.enabled = true;
        LoadAvailableFuses();
    }

    public void LoadAvailableFuses() //spawns and displays the starting fuses in the tray
    {
        PollInventoryForFuses();

        for (; slotsFilled < tray.slots.Length; slotsFilled++)
            if(fusePrefabs[slotsFilled] != null)
            {
                GameObject g = Instantiate(fusePrefabs[slotsFilled], tray.slots[slotsFilled]);
                tray.AssignFilledSlot(g);
            }
    }

    private void PollInventoryForFuses() //checks if the inventory has fuses that are used within this puzzle
    {
        GameObject newFuse = null;

        foreach (string s in nameOfFusesToSearchInInventory)
            if (InventoriesManager.instance.HasItemAndRemove(s, out newFuse))
                fusePrefabs.Add(newFuse);
    }
    #endregion

    #region Lights, strips, and indicator manipulation

    public void ManageColouredStrips(int stripToChange, Color32 col) //set a single strip to a given colour specified in the slotscript
    {
        mainShellMaterials[stripToChange].SetColor("_EmissiveColor", new Color(col.r, col.g, col.b, 4));
    }

    public void ManageColouredStrips(bool correct) //change the colour of every strip at once
    {
        for (int i = 1; i < mainShellMaterials.Count; i++)
        {
            mainShellMaterials[i].SetColor("_EmissiveColor", correct ? new Color(0, 66, 255) : new Color(255, 0, 0));
            ledIndicators[i - 1].material.SetColor("_EmissiveColor", correct ? new Color(0, 66, 255) : new Color(255, 0, 0));
        }
    }

    public void ResetLedIndicators() //turns off every indicator
    {
        for (int i = 0; i < ledIndicators.Count; i++)
            ledIndicators[i].material.SetColor("_EmissiveColor", Color.black);
    }

    private void TurnOnLightsOnSolved()
    {
        hubShell.material = correctSolvedMaterial;
        lightningBolt.material.SetColor("_EmissiveColor", new Color(0, 66, 255));
    }

    #endregion

    #region Text Prompt
    string ITextPrompt.PromptText()
    {
        if (state == PuzzleState.PASSIVE)
        {
            return "This Should Return Power To The Office";
        }
        else
        {
            return "";
        }
    }
    #endregion


    IEnumerator DelaySound(AudioClip clip) //simply delays a sound
    {
        float lapsed = 0.0f;

        while(lapsed < 0.45f)
        {
            lapsed += Time.deltaTime;
            yield return null;
        }

        aud.PlayOneShot(clip);
    }
}
