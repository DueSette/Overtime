using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoGraphPickUp : ItemInGameObjectScript
{
    public GameObject thePhotograph; // The photograph in the world.
    public GameObject sequenceBeginner; // TheGameobject which begins the photograph animation

    private void Start()
    {
        thePhotograph = this.gameObject;
        sequenceBeginner = GameObject.FindGameObjectWithTag("PSequence");
    }

    protected override void InteractionEvent()
    {
        Debug.Log("ActionPressed");
        sequenceBeginner.GetComponent<BoxCollider>().enabled = true;
        thePhotograph.SetActive(false);
    }
}
