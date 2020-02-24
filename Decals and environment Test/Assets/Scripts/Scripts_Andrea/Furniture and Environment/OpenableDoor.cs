using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractable
{
    bool open = false;
    public bool canBeOpened = true;

    [Header("Door Animations")]
    [SerializeField] private Animator doorAnimController;

    [Header("Door Audio")]
    [SerializeField] private AudioClip openingSound;
    [SerializeField] private AudioClip closedSound;
    [SerializeField] private AudioClip lockedSound;
    
    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    void IInteractable.InteractWith() //called when the player clicks the door
    {
        if (!canBeOpened)
        {
            doorAnimController.SetTrigger("DoorLocked");
            aud.PlayOneShot(lockedSound);
            return;
        }

        if (!open)
        {
            doorAnimController.SetTrigger("DoorOpen");
            open = true;
            
            aud.Stop();
            aud.PlayOneShot(openingSound);
        }
        else
        {
            doorAnimController.SetTrigger("DoorClose");
            open = false;
            
            aud.Stop();
            aud.PlayOneShot(closedSound);
        }
    }
}
