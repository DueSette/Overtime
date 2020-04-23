using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPowerScript : MonoBehaviour
{
    [SerializeField] private GameObject computerUI;
    [SerializeField] private Collider interactionCollider; 

    private void OnEnable()
    {
        LevelManager.onLevelEvent += TurnComputerOn;
        LevelManager.onLevelEvent += TurnComputerOff;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= TurnComputerOn;
        LevelManager.onLevelEvent -= TurnComputerOff;
    }

    private void TurnComputerOn(string eventCode)
    {
        if (eventCode == "PowerOn")
        {
            computerUI.SetActive(true);
            interactionCollider.enabled = true;
        }
    }

    private void TurnComputerOff(string eventCode)
    {
        if (eventCode == "PowerOff")
        {
            computerUI.SetActive(false);
            interactionCollider.enabled = false;
        }
    }
}
