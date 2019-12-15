﻿using System.Collections;
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

        theRB.velocity = Vector3.zero;
    }
    protected void MoveElevator()
    {
        if (theRB == null)
        {
            theRB = this.GetComponent<Rigidbody>();
        }

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

        theAnimController.SetTrigger("OpenDoors");
    }

    protected void CloseDoors()
    {
        if (theAnimController == null)
        {
            theAnimController = this.GetComponent<Animator>();
        }

        theAnimController.SetTrigger("CloseDoors");
    }
}