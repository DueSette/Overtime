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

    public void HandFuse(Vector3 point) //equips a fuse to the cursor, selecting the nearest one
    {
        GameObject nearest = null; //default values, never used
        float nearestDist = 9999; 
        int fuseNum = -1;

        for (int i = 0; i < embeddedFuses.Length; i++)
            if (embeddedFuses[i] != null)
            {
                //Check the distance between the point the player clicked and each available fuse
                Vector3 fusePos = embeddedFuses[i].transform.position;
                float dist = Vector3.Distance(fusePos, point);

                if (dist < nearestDist) //Save the nearest fuse and then hand it to the player
                {
                    nearest = embeddedFuses[i];
                    nearestDist = dist;
                    fuseNum = i;
                }              
            }

        if(fuseNum == -1) { return; } //means there are no fuses in the tray

        fuseBox.currentlyHeldFuse = nearest;
        embeddedFuses[fuseNum] = null;
    }

    public void StoreFuse(GameObject fuse) //accept a fuse from the cursor and stores it
    {
        for (int i = 0; i < embeddedFuses.Length; i++)
            if (embeddedFuses[i] == null) //if there is no embedded fuse
            {
                embeddedFuses[i] = fuse;
                fuseBox.currentlyHeldFuse = null;

                StartCoroutine(PlaceFuseOnTray(fuse.transform, slots[i].position));
                return;
            }
    }

    public static IEnumerator PlaceFuseOnTray(Transform fuse, Vector3 endPos) //lerps the fuse from cursor to tray slot
    {
        float lapsed = 0.0f;
        Vector3 startPos = fuse.position;

        while (lapsed <= 1.0f)
        {
            lapsed += Time.deltaTime * 4;
            fuse.position = Vector3.Lerp(startPos, endPos, lapsed * lapsed);

            yield return null;
        }
    }
}
