using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractable
{
    // Unlock Event Information
    public delegate void DoorUnlockEvent(string doorEventCode);
    public static DoorUnlockEvent OnDoorUnlockEvent;
    public bool canBeOpened = true;
    [SerializeField]
    private string doorUnlockEventCode;

    // Default Door Information
    bool open = false;

    [Header("Door Animations")]
    [SerializeField] private Animator doorAnimController;

    [Header("Door Audio")]
    AudioSource audioSource;
    [SerializeField] private AudioClip openingSound;
    [SerializeField] private AudioClip closedSound;
    [SerializeField] private AudioClip lockedSound;


    private void OnEnable()
    {
        OpenableDoor.OnDoorUnlockEvent += UnlockDoor;
    }
    private void OnDisable()
    {
        OpenableDoor.OnDoorUnlockEvent -= UnlockDoor;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    /*
    ====================================================================================================
    Player Interaction
    ====================================================================================================
    */
    void IInteractable.InteractWith() //called when the player clicks the door
    {
        if (!canBeOpened)
        {
            doorAnimController.SetTrigger("DoorLocked");
            audioSource.PlayOneShot(lockedSound);
            return;
        }

        if (!open)
        {
            doorAnimController.SetTrigger("DoorOpen");
            open = true;
            
            audioSource.Stop();
            audioSource.PlayOneShot(openingSound);
        }
        else
        {
            doorAnimController.SetTrigger("DoorClose");
            open = false;
            
            audioSource.Stop();
            audioSource.PlayOneShot(closedSound);
        }
    }


    /*
    ====================================================================================================
    Door Unlocking
    ====================================================================================================
    */
    private void UnlockDoor(string unlockEventCode)
    {
        if (unlockEventCode == doorUnlockEventCode)
        {
            canBeOpened = true;
        }
    }
}
