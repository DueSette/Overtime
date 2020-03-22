using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Image dissolvePanel;
    [SerializeField] AudioSource audio;

    public void LoadWorld(int level)
    {
        StartCoroutine(DissolveAndPlay(level));
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
