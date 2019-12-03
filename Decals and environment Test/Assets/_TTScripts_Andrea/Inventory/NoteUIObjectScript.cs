using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteUIObjectScript : MonoBehaviour
{
    public int noteID = -1;

   public void UpdateUI()
    {
        NoteInventory nMan = GetComponentInParent<NoteInventory>();
        nMan.ScrollEntries(noteID);
    }
}
