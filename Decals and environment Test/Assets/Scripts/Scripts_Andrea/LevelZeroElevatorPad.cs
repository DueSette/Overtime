using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelZeroElevatorPad : MonoBehaviour, IInteractable
{
    bool levelComplete = false;
    bool kidTriggered = false;
    [SerializeField] GameObject kid;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += UnlockElevator;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= UnlockElevator;
    }

    void IInteractable.InteractWith()
    {
        if (levelComplete)
        {
            LevelManager.onLevelEvent("LevelSolved");
        }
        else
        {
            if (!kidTriggered)
            {
                kid.SetActive(true);
                kidTriggered = false;
            }
            GetComponent<AudioSource>().Play();
        }
    }

    private void UnlockElevator(string eventCode)
    {
        if (eventCode == "UnlockElevator")
        {
            this.levelComplete = true;
        }
    }
}