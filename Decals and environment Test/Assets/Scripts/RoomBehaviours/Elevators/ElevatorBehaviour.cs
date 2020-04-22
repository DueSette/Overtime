using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ElevatorBehaviour : MonoBehaviour
{
    // Movement
    public float moveSpeed;
    private Rigidbody theRB;

    // Animation (Door Opening/ Closing)
    private Animator theAnimController;

    // Sound
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip stopSound;


    /*
    ====================================================================================================
    Movement Handling
    ====================================================================================================
    */
    protected void StopElevator()
    {
        if (theRB == null)
        {
            theRB = this.GetComponent<Rigidbody>();
        }
        
        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().PlayOneShot(stopSound);

        theRB.isKinematic = true;
        theRB.velocity = Vector3.zero;

        if (theAnimController == null)
        {
            theAnimController = this.GetComponent<Animator>();
        }

        theAnimController.SetTrigger("Shake");
    }
    protected void MoveElevator()
    {
        if (theRB == null)
        {
            theRB = this.GetComponent<Rigidbody>();
        }
        
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().Play();
        }

        theRB.isKinematic = false;
        theRB.velocity = Vector3.up * moveSpeed;
    }


    /*
    ====================================================================================================
    Animation Handling
    ====================================================================================================
    */

    protected void OpenDoors()
    {
        if (theAnimController == null)
        {
            theAnimController = this.GetComponent<Animator>();
        }

        this.GetComponent<AudioSource>().PlayOneShot(openSound);

        theAnimController.SetTrigger("OpenDoors");
    }

    protected void CloseDoors()
    {
        if (theAnimController == null)
        {
            theAnimController = this.GetComponent<Animator>();
        }

        this.GetComponent<AudioSource>().PlayOneShot(closeSound);

        theAnimController.SetTrigger("CloseDoors");
    }
}