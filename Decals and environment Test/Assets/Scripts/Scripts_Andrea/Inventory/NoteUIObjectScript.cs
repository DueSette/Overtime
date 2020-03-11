using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteUIObjectScript : MonoBehaviour
{
    public int noteID = -1;
    static NoteInventory noteInventoryRef;
    

    public void UpdateUI() //Called via onClick from the Button component
    {
        if(noteInventoryRef == null)
           noteInventoryRef = GetComponentInParent<NoteInventory>();

        noteInventoryRef.ScrollEntries(noteID);
    }

    public void PlaySound(AudioClip clip) //hooked to the onClick button's method
    {
        SoundManager.instance.PlaySound(clip);
    }
}
