using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoGraphPickUp : ItemInGameObjectScript
{
    protected override void InteractionEvent()
    {
        Debug.Log("ActionPressed");


        GameObject[] sequenceBeginner = GameObject.FindGameObjectsWithTag("PSequence");
        foreach(GameObject g in sequenceBeginner)
        {
            g.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
