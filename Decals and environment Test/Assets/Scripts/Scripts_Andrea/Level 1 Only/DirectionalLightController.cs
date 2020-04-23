using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightController : MonoBehaviour
{
    private void OnEnable()
    {
        LevelManager.onLevelEvent += TurnLightOn;
        LevelManager.onLevelEvent += TurnLightOff;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= TurnLightOn;
        LevelManager.onLevelEvent -= TurnLightOff;
    }

    void TurnLightOn(string eventName)
    {
        if (eventName == "MemoryStart")
        {
            this.GetComponent<Light>().enabled = true;
        }
    }

    void TurnLightOff(string eventName)
    {
        if (eventName == "MemoryReturn")
        {
            this.GetComponent<Light>().enabled = false;
        }
    }
}
