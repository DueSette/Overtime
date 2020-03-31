using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FuseTrayScript : MonoBehaviour
{
    /*===========
     * TO BE USED WITHIN A FUSEBOX PUZZLE
    =============*/
    [Tooltip("All the places on the side of the box where unallocated fuses appear")]
    public Transform[] slots;
    private GameObject[] embeddedFuses;

    private FuseBoxScript fuseBox;

    private void Start()
    {
        fuseBox = GetComponentInParent<FuseBoxScript>();

        embeddedFuses = new GameObject[slots.Length];
        enabled = false;
    }

    public void AssignFilledSlot(GameObject g) //this way the tray remembers how many slots have ever been filled (it avoids spawning things twice)
    {
        for (int i = 0; i < embeddedFuses.Length; i++)
            if (embeddedFuses[i] == null) // if there is no embedded fuse
            {
                g.transform.position = slots[i].position;
                embeddedFuses[i] = g;
                return;
            }
    }

    public void HandFuse() //equips a fuse to the cursor
    {
        for (int i = 0; i < embeddedFuses.Length; i++)
            if (embeddedFuses[i] != null) //if there is embedded fuse
            {
                fuseBox.currentlyHeldFuse = embeddedFuses[i];
                embeddedFuses[i] = null;
                return;
            }
    }

    public void StoreFuse(GameObject fuse) //accept a fuse from the cursor and stores it
    {
        for (int i = 0; i < embeddedFuses.Length; i++)
            if (embeddedFuses[i] == null) //if there is no embedded fuse
            {
                embeddedFuses[i] = fuse;
                fuse.transform.position = slots[i].position;
                fuseBox.currentlyHeldFuse = null;
                return;
            }
    }
}
