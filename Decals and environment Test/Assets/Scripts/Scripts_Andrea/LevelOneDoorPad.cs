using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneDoorPad : MonoBehaviour
{
    [SerializeField] private Material padEmissiveMaterial;

    [Header("Unlit Pad Parameters")]
    [SerializeField] private Color unlitColor;
    [SerializeField] private float unlitStrength;

    [Header("Lit Pad Parameters")]
    [SerializeField] private Color litColor;
    [SerializeField] private float litStrength;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += PowerPad;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= PowerPad;
    }

    // Start is called before the first frame update
    void Start()
    {
        padEmissiveMaterial.SetColor("_EmissiveColor", unlitColor);
        padEmissiveMaterial.SetColor("_EmissiveColorLDR", unlitColor);

        padEmissiveMaterial.SetFloat("_EmissiveIntensity", unlitStrength);
    }

    void PowerPad(string eventCode)
    {
        if (eventCode == "FuseBoxPuzzleSolved")
        {
            padEmissiveMaterial.SetColor("_EmissiveColor", litColor);
            padEmissiveMaterial.SetColor("_EmissiveColorLDR", litColor);

            padEmissiveMaterial.SetFloat("_EmissiveIntensity", litStrength);
        }
    }
}
