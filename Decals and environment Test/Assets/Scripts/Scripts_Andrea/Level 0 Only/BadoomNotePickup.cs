using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BadoomNotePickup : NoteInGameObjectScript
{
    [SerializeField] GameObject[] badooms;
    [SerializeField] GameObject[] emissiveMaterialsToSwap;
    [SerializeField] Material materialToBeSwappedIn;

    [SerializeField] Light[] lightSourcesToTurnOff;
    [SerializeField] GameObject darkPostProcess;

    private Material originalEmissiveMaterial;
    private float[] originalIntensities;

    protected override void OnInteraction()
    {
        base.OnInteraction();
        SetupBackupReferences();

        //Starts badoom sequence
        ActivateBadooms(true);
        TurnLightsOff(true);
        EnableDarkPostProcess(true);
    }

    private void SetupBackupReferences()
    {
        if (materialToBeSwappedIn == null)
            return;

        materialToBeSwappedIn = emissiveMaterialsToSwap[0].GetComponent<Renderer>().material; //take a reference to reinstate the previous emissive materials
        originalIntensities = new float[lightSourcesToTurnOff.Length];

        for (int i = 0; i < lightSourcesToTurnOff.Length; i++)        
            originalIntensities[i] = lightSourcesToTurnOff[i].intensity;     
    }

    public void EndBadoomSequence() //brings everything backtonormal, should happen offscreen
    {
        ActivateBadooms(false);
        TurnLightsOff(false);
        EnableDarkPostProcess(false);
    }

    void ActivateBadooms(bool start)
    {
        foreach (GameObject bad in badooms)
            bad.SetActive(start);
    }

    void TurnLightsOff(bool start)
    {
        foreach (GameObject emissiveObject in emissiveMaterialsToSwap)
        {
            emissiveObject.GetComponent<Renderer>().material = start ? materialToBeSwappedIn : originalEmissiveMaterial;
        }
        for (int i = 0; i < lightSourcesToTurnOff.Length; i++)
        {
            Light light = lightSourcesToTurnOff[i];
            light.intensity = start ? 0 : originalIntensities[i];
        }
    }

    void EnableDarkPostProcess(bool start)
    {
        darkPostProcess?.SetActive(start);
    }
}
