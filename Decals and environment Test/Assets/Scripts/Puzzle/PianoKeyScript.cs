using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyScript : MonoBehaviour, IInteractable
{

    [SerializeField] private bool noteActive;
    [SerializeField] private AudioClip fAud, gAud, eAud, aAud;
    [SerializeField] private int noteNumber;
    [SerializeField] AudioSource aud;
    [SerializeField] private PianoNoteScript pnScript;
    public GameObject thePiano;


    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void IInteractable.InteractWith()
    {
        if (this.gameObject.tag == "FNote")
        {
            aud.PlayOneShot(fAud);
            noteNumber = 1;
   //         Debug.Log("F Pressed");
            UpdateList();
        }
        else
         if (this.gameObject.tag == "GNote")
        {
            aud.PlayOneShot(gAud);
            noteNumber = 2;
     //       Debug.Log("G Pressed");
            UpdateList();
        }
        else
         if (this.gameObject.tag == "ENote")
        {
            aud.PlayOneShot(eAud);
            noteNumber = 3;
     //       Debug.Log("E Pressed");
            UpdateList();
        }
        else
         if (this.gameObject.tag == "ANote")
        {
            aud.PlayOneShot(aAud);
            noteNumber = 4;
      //      Debug.Log("A Pressed");
            UpdateList();
        }
    }

    void UpdateList()
    {
        pnScript = thePiano.GetComponent<PianoNoteScript>();
        pnScript.musicNotes.Add(noteNumber);
        pnScript.active = true;
    }
}
