using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LevelLightsController : MonoBehaviour
{
    [Header("Default Light Vals")]
    [SerializeField] private float defaultLightStrength;
    [SerializeField] private Color defaultLightColor;

    [Header("Dimmed Light Animating")]
    [SerializeField] private float dimmedLightStrength;
    [SerializeField] private Color dimmedLightColor1;
    [SerializeField] private Color dimmedLightColor2;

    private Light light;
    private bool lowPower = true;
    [SerializeField] private AnimationCurve blinkCurve;
    [SerializeField] private float animTime;
    private float currentAnimTime;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += DefaultLights;
    }
    private void OnDisable()
    {
        LevelManager.onLevelEvent -= DefaultLights;
    }

    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponent<Light>();
    }

    private void Update()
    {
        if (lowPower)
        {
            currentAnimTime += (Time.deltaTime / animTime);
            if (currentAnimTime > 1)
            {
                currentAnimTime -= 1;
            }

            // Get light Strength at current time
            float strength = blinkCurve.Evaluate(currentAnimTime);
            light.color = Color.Lerp(dimmedLightColor1, dimmedLightColor2, strength);
        }
    }

    private void DimLights(string eventCode)
    {
        if (eventCode == "")
        {
            light.intensity = dimmedLightStrength;
        }
    }
    private void DefaultLights(string eventCode)
    {
        if (eventCode == "FuseBoxPuzzleSolved")
        {
            light.intensity = defaultLightStrength;
        }
    }
}