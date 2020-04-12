using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelZeroElevatorPad : MonoBehaviour, IInteractable
{
    [Header("Event Handling")]
    private bool levelComplete = false;
    private bool kidTriggered = false;
    private bool endTriggered = false;
    [SerializeField] GameObject kid;

    [Header("Animations")]
    [SerializeField] private Material sharedEmissiveMaterial;
    [SerializeField] private Color defaultColor;

    [Header("Fail Animation")]
    [SerializeField] private Color litRed;
    [SerializeField] private Color unlitRed;
    [SerializeField] private uint numberOfBlinks;
    [SerializeField] private float litTime;
    [SerializeField] private float timeBetweenLit;

    private void Start()
    {
        SetEmissiveMaterialColor(defaultColor);
    }

    /*
    ====================================================================================================
    Event Handling
    ====================================================================================================
    */
    private void OnEnable()
    {
        LevelManager.onLevelEvent += UnlockElevator;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= UnlockElevator;
    }


    /*
    ====================================================================================================
    Interaction Handling
    ====================================================================================================
    */
    void IInteractable.InteractWith()
    {
        if (levelComplete)
        {
            if (!endTriggered)
            {
                // Elevator Opening Event
                LevelManager.onLevelEvent("LevelSolved");
                endTriggered = true;

                // Normal Pad Success Animation
                StartCoroutine(PadSuccessAnimation());
            }
        }
        else
        {
            if (!kidTriggered)
            {
                // Kid Spawning Event
                kid.SetActive(true);
                kidTriggered = true;
            }

            // Normal Pad Fail Animation
            GetComponent<AudioSource>().Play();
            StartCoroutine(PadFailAnimation());
        }
    }


    /*
    ====================================================================================================
    Pad Behaviour
    ====================================================================================================
    */
    public void SetEmissiveMaterialColor(Color newColor)
    {
        sharedEmissiveMaterial.SetColor("_EmissiveColor", newColor);
        sharedEmissiveMaterial.SetColor("_EmissiveColorLDR", newColor);
    }

    public void SetEmissiveMaterialColor(Color newColor, float timeToChange)
    {
        StartCoroutine(LerpEmissiveMaterial(sharedEmissiveMaterial.color, newColor, timeToChange));
    }

    private IEnumerator LerpEmissiveMaterial(Color startColor, Color endColor, float timeToChange)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += (Time.deltaTime / timeToChange);

            Color newColor = Color.Lerp(startColor, endColor, t);
            SetEmissiveMaterialColor(newColor);

            yield return null;
        }
    }

    private void UnlockElevator(string eventCode)
    {
        if (eventCode == "UnlockElevator")
        {
            this.levelComplete = true;
        }
    }


    /*
    ====================================================================================================
    Pad Animations
    ====================================================================================================
    */
    private IEnumerator PadFailAnimation()
    {
        // Gets The Pad To Blink Red Three Times
        for (int i = 0; i < numberOfBlinks; i++)
        {
            SetEmissiveMaterialColor(litRed);
            yield return new WaitForSeconds(litTime);

            SetEmissiveMaterialColor(unlitRed);
            yield return new WaitForSeconds(timeBetweenLit);

            yield return null;
        }

        SetEmissiveMaterialColor(defaultColor);
    }

    private IEnumerator PadSuccessAnimation()
    {
        yield return null;
    }
}