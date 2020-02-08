using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteUIObjectScript : MonoBehaviour
{
    public int noteID = -1;
    static NoteInventory noteInventoryRef;

   public void UpdateUI()
    {
        if(noteInventoryRef == null)
           noteInventoryRef = GetComponentInParent<NoteInventory>();

        noteInventoryRef.ScrollEntries(noteID);
    }

    public void PlaySound(AudioClip clip) //hooked to the OnClick button's method
    {
        SoundManager.instance.PlaySound(clip);
    }
}
