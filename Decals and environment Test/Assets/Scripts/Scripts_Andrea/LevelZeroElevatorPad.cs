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
    [SerializeField] private AudioClip failSFX;

    [Header("Success Animation")]
    [SerializeField] private Color litGreen;
    [SerializeField] private Color unlitGreen;
    [SerializeField] private AudioClip successSFX;

    [Header("Music Changing")]
    [SerializeField] AudioClip newBackgroundMusic;
    [SerializeField] float fadeOutTime = 3;
    [SerializeField] float fadeInTime = 3;

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
                GetComponent<AudioSource>().PlayOneShot(successSFX);
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

                // Kid Music
                SoundManager.instance.FadeBGM(newBackgroundMusic, fadeOutTime, fadeInTime);
            }

            // Normal Pad Fail Animation
            GetComponent<AudioSource>().PlayOneShot(failSFX);
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
    public IEnumerator PadFailAnimation()
    {
        // Gets The Pad To Blink Red Three Times
        for (int i = 0; i < numberOfBlinks; i++)
        {
            SetEmissiveMaterialColor(litRed);
            yield return new WaitForSeconds(litTime);

            SetEmissiveMaterialColor(unlitRed);
            yield return new WaitForSeconds(timeBetweenLit);
        }

        StartCoroutine(LerpEmissiveMaterial(unlitRed, defaultColor, 0.5f));
        yield return new WaitForSeconds(0.5f);

        SetEmissiveMaterialColor(defaultColor);
    }

    public IEnumerator PadSuccessAnimation()
    {
        // Gets The Pad To Blink Red Three Times
        for (int i = 0; i < numberOfBlinks; i++)
        {
            SetEmissiveMaterialColor(litGreen);
            yield return new WaitForSeconds(litTime);

            SetEmissiveMaterialColor(unlitGreen);
            yield return new WaitForSeconds(timeBetweenLit);
        }

        SetEmissiveMaterialColor(litGreen);
    }
}