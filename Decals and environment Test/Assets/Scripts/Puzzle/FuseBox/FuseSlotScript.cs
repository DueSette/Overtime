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
    [SerializeField] string correctFuseColor;
    [SerializeField] AudioClip putIntoFuseSlot, retrieveFromFuseSlot;
    bool isFilled;

    private FuseBoxScript fuseBox;
    private AudioSource aud;
    static int correctFuses = 0;

    private void Start()
    {
        fuseBox = GetComponentInParent<FuseBoxScript>();
        aud = GetComponent<AudioSource>();

        isFilled = containedFuse != null ? true : false;
        if (isFilled)
            containedFuse.GetComponent<Collider>().enabled = false;
    }

    public void Interact()
    {
        if (isFilled)
        {
            aud.PlayOneShot(retrieveFromFuseSlot);
            ExtractFuse();
        }
        else
        {
            aud.PlayOneShot(putIntoFuseSlot);
            EmbedFuse();
        }
    }

    void EmbedFuse() //puts the fuse the player is holding in the slot
    {
        if (fuseBox.currentlyHeldFuse == null) { return; }

        isFilled = true;

        containedFuse = fuseBox.currentlyHeldFuse;
        if (CheckFuse()) { correctFuses++; }

        StartCoroutine(FuseTrayScript.PlaceFuseOnTray(containedFuse.transform, transform.position));
        containedFuse.GetComponent<Collider>().enabled = false;

        fuseBox.currentlyHeldFuse = null;

        if(correctFuses == 4) { fuseBox.SetSolvedState(); }
    }

    void ExtractFuse() //extracts the fuse from the slot and hands it to the player
    {
        if(fuseBox.currentlyHeldFuse != null) { return; }

        isFilled = false;

        if(CheckFuse()) { correctFuses--; }

        containedFuse.GetComponent<Collider>().enabled = false;
        fuseBox.currentlyHeldFuse = containedFuse;
        containedFuse = null;
    }

    bool CheckFuse()
    {
        return containedFuse.name.Contains(correctFuseColor);
    }
}
