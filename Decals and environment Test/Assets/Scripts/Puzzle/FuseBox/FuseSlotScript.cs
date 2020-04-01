using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class FuseSlotScript : MonoBehaviour
{
    /*===========
     * TO BE USED WITHIN A FUSEBOX PUZZLE
    =============*/

    [SerializeField] GameObject lightIndicator;
    [SerializeField] GameObject containedFuse;
    bool isFilled;

    private FuseBoxScript fuseBox;

    private void Start()
    {
        fuseBox = GetComponentInParent<FuseBoxScript>();
        isFilled = containedFuse != null ? true : false;
        if (isFilled)
            containedFuse.GetComponent<Collider>().enabled = false;
    }

    public void Interact()
    {
        if (isFilled)
            ExtractFuse();
        else
            EmbedFuse();
    }

    void EmbedFuse() //puts the fuse the player is holding in the slot
    {
        if (fuseBox.currentlyHeldFuse == null) { return; }

        isFilled = true;

        containedFuse = fuseBox.currentlyHeldFuse;
        containedFuse.transform.position = transform.position;
        containedFuse.GetComponent<Collider>().enabled = false;

        fuseBox.currentlyHeldFuse = null;
    }

    void ExtractFuse()
    {
        if(fuseBox.currentlyHeldFuse != null) { return; }

        isFilled = false;

        containedFuse.GetComponent<Collider>().enabled = false;
        fuseBox.currentlyHeldFuse = containedFuse;
        containedFuse = null;
    }
}
