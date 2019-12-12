using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoGraphPickUp : MonoBehaviour
{
    public float theDistance;
    public GameObject thePhotograph; // The photograph in the world.
    public GameObject sequenceBeginner; // TheGameobject which begins the photograph animation

    private void Start()
    {
        thePhotograph = this.gameObject;
        sequenceBeginner = GameObject.FindGameObjectWithTag("PSequence");
    }

    // Update is called once per frame
    void Update()
    {
        theDistance = PlayerCasting.distanceFromTarget; // TheDistance is as far as the player is from the target.
    }

    void OnMouseOver() // When mouse is over the object this script is attatched to.
    {
        if (Input.GetKeyDown(KeyCode.E)) // If player presses E -
        {

            Debug.Log("ActionPressed");
            if (theDistance <= 3) //  - Whilst less than a distance of 3 from door.
            {
                sequenceBeginner.GetComponent<BoxCollider>().enabled = true;
                thePhotograph.SetActive(false);
            }
        }
    }
}
