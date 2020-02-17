using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoNoteScript : MonoBehaviour
{

    public List<int> musicNotes;
    public bool active = false;
    public int currentNote;
    public GameObject thePiano;
    public AudioSource aud;
    public AudioClip drawerSound;
    public Animation keyAnimation;
    private int listLength;
    public int desiredNoteOne = 4, desiredNoteTwo = 1, 
               desiredNoteThree = 2, desiredNote4 = 3;

    // 4 1 2 3
    // A, F, G, E 

    private void Start()
    {
        thePiano = this.gameObject;
        aud = GetComponent<AudioSource>();
    }


    void Update()
    {
        listLength = musicNotes.Count;

        if (active)
        {
            active = false;


            if (listLength >= 1)
            {
                currentNote = musicNotes[listLength - 1];
            }

            if (listLength == 1)
            {
                if (currentNote != 4)
                {
                    musicNotes.Clear();
                    Debug.Log(" list cleared ");
                }
            }

            if (listLength == 2)
            {
                if (currentNote != 1)
                {
                    musicNotes.Clear();
                    Debug.Log(" list cleared ");
                }
            }

            if (listLength == 3)
            {
                if (currentNote != 2)
                {
                    musicNotes.Clear();
                    Debug.Log(" list cleared ");
                }
            }

            if (listLength == 4)
            {
                if (currentNote != 3)
                {
                    musicNotes.Clear();
                    Debug.Log(" list cleared ");
                }
                else
                {
                    Debug.Log(" Hi there ");
                    thePiano.GetComponent<Animation>().Play("BlockMove");
                    aud.PlayOneShot(drawerSound);
                }
            }

            if (listLength > 4)
            {
                musicNotes.Clear();
                Debug.Log("list limit reached");
            }

            active = false;
        }

    }

    void UpdateCurrentList()
    {
        if (musicNotes.Count == 0)
        {
           // Debug.Log("zero in list");
        }
        else
        {
          //  Debug.Log("" + musicNotes.Count + "is the adjusted amount in list");
        }
    }
}
