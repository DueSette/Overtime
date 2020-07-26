using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicChanger : MonoBehaviour
{
    [SerializeField] AudioClip newBackgroundMusic;
    [SerializeField] float fadeOutTime = 3;
    [SerializeField] float fadeInTime = 3;
    [SerializeField] bool restorePreviousMusicOnExit;
    [SerializeField] bool destroyOnExit;

    private void OnTriggerEnter(Collider other)
    {
        SoundManager.instance.FadeBGM(newBackgroundMusic, fadeOutTime, fadeInTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if(restorePreviousMusicOnExit)
        {
            SoundManager.instance.RestorePreviousBGM(fadeOutTime, fadeInTime);
        }
     
        if (destroyOnExit)
        {
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
