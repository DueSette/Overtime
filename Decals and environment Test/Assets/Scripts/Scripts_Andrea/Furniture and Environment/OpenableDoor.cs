using System.Collections;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractable
{
    // Unlock Event Information
    public delegate void DoorUnlockEvent(string doorEventCode);
    public static DoorUnlockEvent OnDoorUnlockEvent;
    [SerializeField] private string doorUnlockEventCode;
    public bool canBeOpened = true;

    // Default Door Information
    bool open = false;
    bool canInteract = true; // Temp var to prevent double clicking doors

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
        if (canInteract)
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

            StartCoroutine(PreventDoubleClick());
        }
    }

    /*
    ====================================================================================================
    Door Unlocking
    ====================================================================================================
    */
    public void LockDoor(string newLockCode)
    {
        doorAnimController.SetTrigger("DoorForceClose");

        doorUnlockEventCode = newLockCode;
        canBeOpened = false;
    }

    private void UnlockDoor(string unlockEventCode)
    {
        if (unlockEventCode == doorUnlockEventCode)
        {
            Debug.Log("Unlocking Door: " + doorUnlockEventCode);
            canBeOpened = true;
        }
    }

    private IEnumerator PreventDoubleClick()
    {
        canInteract = false;

        yield return new WaitForSeconds(2.0f);

        canInteract = true;
    }
}
