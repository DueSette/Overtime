using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour, IInteractable
{
    public bool isPlaying;
    public AudioSource aud;
    public AudioClip music;


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
        if (!isPlaying)
        {
            aud.PlayOneShot(music);
            isPlaying = true;
        }
        else
        {
            aud.Stop();
            isPlaying = false;
        }
    }
}
