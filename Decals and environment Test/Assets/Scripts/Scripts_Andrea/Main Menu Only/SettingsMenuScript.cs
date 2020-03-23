using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float startMasterVolume;
    private float startMusicVolume;
    private float startSfxVolume;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    private void OnEnable()
    {
        gameMixer.GetFloat("Master", out startMasterVolume);
        gameMixer.GetFloat("Music", out startMusicVolume);
        gameMixer.GetFloat("SFX", out startSfxVolume);

        masterSlider.value = ConvertToSliderValue(startMasterVolume);
        musicSlider.value = ConvertToSliderValue(startMusicVolume);
        sfxSlider.value = ConvertToSliderValue(startSfxVolume);
    }


    /*
    ====================================================================================================
    Controlling Game Volume
    ====================================================================================================
    */
    public void SetMasterVolume()
    {
        gameMixer.SetFloat("Master", ConvertToDB(masterSlider.value));
    }
    public void SetMusicVolume()
    {
        gameMixer.SetFloat("Music", ConvertToDB(musicSlider.value));
    }
    public void SetSfxVolume()
    {
        gameMixer.SetFloat("SFX", ConvertToDB(sfxSlider.value));
    }


    /*
    ====================================================================================================
    Exiting Settings Menu
    ====================================================================================================
    */
    public void CancelSettings()
    {
        gameMixer.SetFloat("Master", startMasterVolume);
        gameMixer.SetFloat("Music", startMusicVolume);
        gameMixer.SetFloat("SFX", startSfxVolume);

        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SaveSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    /*
    ====================================================================================================
    Utility
    ====================================================================================================
    */
    private float ConvertToDB(float sliderValue)
    {
        float db = Mathf.Log(sliderValue);
        return  (db * 20);
    }

    private float ConvertToSliderValue(float db)
    {
        float sliderValue = db / 20;
        return Mathf.Exp(sliderValue);
    }
}
