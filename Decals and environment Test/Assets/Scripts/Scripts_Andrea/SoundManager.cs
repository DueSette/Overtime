using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private List<AudioSource> sources = new List<AudioSource>();
    private void Awake()
    {
        if (instance == null)
            instance = this;

        sources.Add(GetComponent<AudioSource>());
    }

    public void PlaySound(AudioClip clip)
    {
        //if (currentlyPlayingClip == clip) can cause strange bugs related to audio sound muffled
           // return;

        FindAvailableSource().PlayOneShot(clip);
    }
    private AudioSource FindAvailableSource()
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (!sources[i].isPlaying)
                return sources[i];
        }

        sources.Add(gameObject.AddComponent<AudioSource>());
        sources[sources.Count - 1].playOnAwake = false;
        return sources[sources.Count - 1];
    }
}
