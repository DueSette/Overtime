using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxScript : MonoBehaviour, IInteractable
{
    /*===========
     * SELF CONTAINED CLASS FOR FUSEBOX PUZZLE LOGIC
    =============*/
    [SerializeField, Tooltip("When the player interacts, the game searches for the fuses in the inventory using the name of the item(s)")]
    string[] nameOfFusesToSearchInInventory;

    [SerializeField, Tooltip(" each fuse object that STARTS within the box - do not put inventory fuses here")]
    List<GameObject> fusePrefabs = new List<GameObject>();

    FuseSlotScript[] fuseSlots; // all the places where you can fit a fuse in, they are classes as they contain a package of info

    FuseTrayScript tray;

    [SerializeField] LayerMask fuseBoxLayer;
    [HideInInspector] public GameObject currentlyHeldFuse = null; //the fuse the player is currently using

    bool inUse = false;
    int slotsFilled = 0; //used for keeping track of how many items have been spawned on the tray, internal logic
    private float cameraDist;

    private void Start()
    {
        tray = GetComponentInChildren<FuseTrayScript>();

        int i = 0;
        var temp = new List<FuseSlotScript>();
        temp.AddRange(GetComponentsInChildren<FuseSlotScript>());

        foreach (FuseSlotScript f in temp)
        {
            fuseSlots = new FuseSlotScript[temp.Count];
            fuseSlots[i] = f;
            i++;
        }
    }

    private void Update()
    {
        if (!inUse) { return; }

        if (Input.GetButtonUp("Fire1")) { CheckFuseboxInteraction(); }

        if (currentlyHeldFuse != null) { UpdateHeldFusePosition(); }
    }

    void CheckFuseboxInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cameraDist = Vector3.Distance(transform.position, GameStateManager.GetPlayer().transform.position) / 3;

        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f, fuseBoxLayer))
        {
            FuseSlotScript f = hit.collider.gameObject.GetComponent<FuseSlotScript>();

            if (f != null) //did we hit a fuseslot?
                f.Interact();

            else if(hit.collider.name.Contains("Tray"))
            {
                //TODO tray should hand a fuse to the controller
                //tray should keep track of which tray slots are empty and which aren't
                if (currentlyHeldFuse == null)
                    tray.HandFuse();
                else
                    tray.StoreFuse(currentlyHeldFuse);
            }
        }
    }

    void UpdateHeldFusePosition()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist));

        currentlyHeldFuse.transform.position = v;
    }

    #region Startup Methods
    void IInteractable.InteractWith()
    {
        if(inUse) { return; }

        inUse = true;
        //should also play animation
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the fusebox in focus

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadAvailableFuses();
    }

    public void LoadAvailableFuses() //spawns and displays the starting fuses in the tray
    {
        PollInventoryForFuses();

        for (; slotsFilled < tray.slots.Length; slotsFilled++)
            if(fusePrefabs[slotsFilled] != null)
            {
                GameObject g = Instantiate(fusePrefabs[slotsFilled], tray.slots[slotsFilled]);
                //g.transform.position = tray.slots[slotsFilled].position;
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

    private void OnEnable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += ExitInteraction;
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= ExitInteraction;
    }
    
    void ExitInteraction()
    {
        inUse = false;
    }

    #endregion
    /*
     * TODO: ONLEAVE() LOGICS:
     * DEACTIVATE ALL RELEVANT THINGS (SLOTS, FUSES)
     * WORRY ABOUT STUFF THAT MIGHT GET LOADED TWICE
     * PUT DOWN ANY HELD FUSE (TRAY OR INVENTORY?)
     */
}
