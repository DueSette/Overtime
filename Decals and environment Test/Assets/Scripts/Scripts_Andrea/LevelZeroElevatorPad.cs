using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelZeroElevatorPad : MonoBehaviour, IInteractable
{
    bool once = true;
    [SerializeField] GameObject kid;

    void IInteractable.InteractWith()
    {
        if(once)
        {
            kid.SetActive(true);
            once = false;
        }
        GetComponent<AudioSource>().Play();
    }
}
