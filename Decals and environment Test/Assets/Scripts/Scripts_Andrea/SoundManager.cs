using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource aud;
    AudioClip currentlyPlayingClip;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        aud = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (currentlyPlayingClip == clip)
            return;

        aud.PlayOneShot(clip);
        StartCoroutine(ManageLastClip(clip));
    }

    private IEnumerator ManageLastClip(AudioClip clip)
    {
        currentlyPlayingClip = clip;
        yield return new WaitForSeconds(currentlyPlayingClip.length);
        currentlyPlayingClip = null;
    }
}
