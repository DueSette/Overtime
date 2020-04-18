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
        if (Check()) { return; }

        volume = GetComponent<Volume>();
        previousProfile = volume.profile;
    }

    public void ChangeToPause(VolumeProfile newProf)
    {
        if (Check()) { return; }

        previousProfile = volume.profile;
        volume.profile = newProf;
    }

    public void RevertToPreviousProfile()
    {
        if (Check()) { return; }

        volume.profile = previousProfile;
    }

    bool Check() //with this function, if the SceneSettings object that contains the general post process stack volume is not present, it doesn't throw errors
    {
        return gameObject != null;
    }
}
