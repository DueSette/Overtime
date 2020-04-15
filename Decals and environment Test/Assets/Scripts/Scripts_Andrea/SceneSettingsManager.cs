using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class SceneSettingsManager : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile previousProfile;
    public static SceneSettingsManager instance;

    void Awake()
    {
        if (instance == null) { instance = this; }

        volume = GetComponent<Volume>();
        previousProfile = volume.profile;
    }

    public void ChangeToPause(VolumeProfile newProf)
    {
        previousProfile = volume.profile;
        volume.profile = newProf;
    }

    public void RevertToPreviousProfile()
    {
        volume.profile = previousProfile;
    }
}
