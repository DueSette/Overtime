using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] Button continueButton;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] Image dissolvePanel;
    [SerializeField] AudioSource audio;

    private void Start()
    {
        Debug.developerConsoleVisible = false;

        int i = PlayerPrefs.GetInt("CanContinue");
        if (i == 1)
        {
            continueButton.interactable = true;
        }
    }

    public void LoadWorld(int level)
    {
        StartCoroutine(DissolveAndPlay(level));
    }
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void QuitGame()
    {
        StartCoroutine(DissolveAndQuit());
    }

    private IEnumerator DissolveAndPlay(int level)
    {
        Color startColor = dissolvePanel.color;
        float startVolume = audio.volume;
        float lapsed = 0f;
        
        while (lapsed < 1.0f)
        {
            lapsed += Time.deltaTime / 2;
            dissolvePanel.color = Color.Lerp(startColor, new Color(0, 0, 0, 1), lapsed);
            audio.volume = Mathf.Lerp(startVolume, 0, lapsed + 0.15f);
            yield return null;
        }

        SceneManager.LoadSceneAsync(level);
    }

    private IEnumerator DissolveAndQuit()
    {
        Color startColor = dissolvePanel.color;
        float startVolume = audio.volume;
        float lapsed = 0f;

        while (lapsed < 1.0f)
        {
            lapsed += Time.deltaTime / 2;
            dissolvePanel.color = Color.Lerp(startColor, new Color(0, 0, 0, 1), lapsed);
            audio.volume = Mathf.Lerp(startVolume, 0, lapsed + 0.15f);
            yield return null;
        }

        Application.Quit();
    }
}
