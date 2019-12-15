using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractable
{
    bool open = false;
    [SerializeField] bool canBeOpened = true;
    [SerializeField] private bool flipRotationDirection;
    [SerializeField] GameObject hingePivot;

    [SerializeField] private AudioClip openingSound;
    [SerializeField] private AudioClip closedSound;
    [SerializeField] private AudioClip lockedSound;
    
    AudioSource aud;
    float lapsed = 0;
    Coroutine rotRoutine;

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    void IInteractable.InteractWith() //called when the player clicks the door
    {
        if (!canBeOpened)
        {
            aud.PlayOneShot(lockedSound);
            return;
        }

        if (rotRoutine != null)
        {
            StopCoroutine(rotRoutine); //interrupt door movement if the player clicks the door while it is still moving
            lapsed = 1 - lapsed; //adjust the value the lerp is based on
        }

        if (!open)
            rotRoutine = StartCoroutine(RotateDoorOnHinge(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, -90 * (flipRotationDirection ? -1 : 1), 0)));
        else
            rotRoutine = StartCoroutine(RotateDoorOnHinge(Quaternion.Euler(0, -90 * (flipRotationDirection ? -1 : 1), 0), Quaternion.Euler(0, 0, 0))); //TODO USE THIS AS FIRST VALUE : Quaternion.Euler(0, 0, 0)
    }

    private IEnumerator RotateDoorOnHinge(Quaternion startRot, Quaternion endRot)
    {
        open = !open;
        aud.Stop();
        aud.PlayOneShot(openingSound);

        while (lapsed < 1)
        {
            if (lapsed + Time.deltaTime > 1)
                lapsed = 1;
            else
                lapsed += Time.deltaTime;

            float f = Mathf.Lerp(lapsed * lapsed, 1 - (1 - lapsed) * (1 - lapsed), lapsed); //for a smooth lerp

            Quaternion rot = Quaternion.Lerp(startRot, endRot, f);
            hingePivot.transform.localRotation = rot;

            yield return new WaitForEndOfFrame();        
        }

        if (hingePivot.transform.localRotation.eulerAngles.y == 0)
            aud.PlayOneShot(closedSound);

        rotRoutine = null;
        lapsed = 0;

        yield return null;
    }
}
