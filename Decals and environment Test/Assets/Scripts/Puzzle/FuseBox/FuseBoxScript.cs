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

    [SerializeField, Tooltip("Each fuse object that STARTS within the box - do not put inventory fuses here")]
    List<GameObject> fusePrefabs = new List<GameObject>();

    FuseSlotScript[] fuseSlots; // all the places where you can fit a fuse in, they are classes as they contain a package of info

    FuseTrayScript tray;

    [SerializeField] LayerMask fuseBoxLayer;
    [HideInInspector] public GameObject currentlyHeldFuse = null; //the fuse the player is currently using

    bool inUse = false;
    int slotsFilled = 0; //used for keeping track of how many items have been spawned on the tray, internal logic
    private float cameraDist; //will be used as the distance between the player cam and the held fusebox

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

    #region Interaction
    void CheckFuseboxInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        cameraDist = Vector3.Distance(transform.position, GameStateManager.GetPlayer().transform.position) / 3; //updates the distance from the fusebox to the camera

        if (Physics.Raycast(ray, out RaycastHit hit, 2.5f, fuseBoxLayer))
        {
            FuseSlotScript f = hit.collider.gameObject.GetComponent<FuseSlotScript>();

            if (f != null) //did we hit a fuseslot?
                f.Interact();

            else if(hit.collider.name.Contains("Tray"))
            {
                if (currentlyHeldFuse == null)
                    tray.HandFuse();
                else
                    tray.StoreFuse(currentlyHeldFuse);
            }
        }
    }

    void UpdateHeldFusePosition() //makes the fuse follow the cursor on the X and Y, uses a fixed Z distance
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist));
        Vector3 vel = Vector3.zero;

        currentlyHeldFuse.transform.position = Vector3.SmoothDamp(currentlyHeldFuse.transform.position, v, ref vel, 0.025f);
    }

    #endregion

    #region Startup Methods
    void IInteractable.InteractWith()
    {
        if(inUse) { return; }

        inUse = true;
        //should also play animation
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the fusebox in focus

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
        if (currentlyHeldFuse != null)
            tray.StoreFuse(currentlyHeldFuse);

        tray.enabled = false; //prevents mishaps from happening
    }

    #endregion
}
