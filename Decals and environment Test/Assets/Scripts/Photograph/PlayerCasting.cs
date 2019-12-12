using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{

    public static float distanceFromTarget; // Variable for other classes to access.
    public float toTarget; // This is used to keep the value of Distance from target shown in the inspector.
    public Transform target;
    public static Transform cameraTarget;

    void Update()
    {

        RaycastHit Hit; // Creates a raycast called hit.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Hit)) ; // If the raycast hits an object in front of it
        {
            toTarget = Hit.distance; // Assign the toTarget variable the same amount as the Hit output
            distanceFromTarget = toTarget; // Assign the Distancefromtarget variable the same value as the ToTarget variable(Same as Hit output)
        }

        cameraTarget = target;

    }
}
