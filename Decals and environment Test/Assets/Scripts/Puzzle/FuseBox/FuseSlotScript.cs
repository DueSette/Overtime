using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class FuseSlotScript : MonoBehaviour
{
    /*===========
     * TO BE USED WITHIN A FUSEBOX PUZZLE
    =============*/

    [SerializeField] int slotNumber;
    [SerializeField] Color litColor;
    GameObject containedFuse;
    [SerializeField] string correctFuseColor;
    [SerializeField] AudioClip putIntoFuseSlot, retrieveFromFuseSlot;
    bool isFilled;

    private FuseBoxScript fuseBox;
    private AudioSource aud;

    static int correctFuses = 0;
    static int filledSlots = 0;

    private void Start()
    {
        fuseBox = GetComponentInParent<FuseBoxScript>();
        aud = GetComponent<AudioSource>();

        isFilled = (containedFuse != null);
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
        filledSlots++;

        if (CheckFuse())
            correctFuses++;

        StartCoroutine(FuseTrayScript.PlaceFuseOnTray(containedFuse.transform, transform.position));
        containedFuse.GetComponent<Collider>().enabled = false;

        fuseBox.currentlyHeldFuse = null;

        fuseBox.ManageColouredStrips(slotNumber, litColor);

        if(filledSlots == 4)
        {
            if (correctFuses == 4)
                StartCoroutine(fuseBox.SetSolvedState());
            else
                fuseBox.ManageColouredStrips(false);
        }
    }

    void ExtractFuse() //extracts the fuse from the slot and hands it to the player
    {
        if(fuseBox.currentlyHeldFuse != null) { return; }

        isFilled = false;

        filledSlots--;
        fuseBox.ResetLedIndicators();

        if(CheckFuse()) { correctFuses--; }

        containedFuse.GetComponent<Collider>().enabled = false;
        fuseBox.currentlyHeldFuse = containedFuse;
        containedFuse = null;
        fuseBox.ManageColouredStrips(slotNumber, Color.black);
    }

    bool CheckFuse()
    {
        return containedFuse.name.Contains(correctFuseColor);
    }
}
