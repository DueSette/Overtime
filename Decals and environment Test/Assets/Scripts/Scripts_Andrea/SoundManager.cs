using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource bgmA, bgmB;

    private AudioClip previousBGM;
    private List<AudioSource> sources = new List<AudioSource>();

    public void PlaySound(AudioClip clip)
    {
        FindAvailableSource().PlayOneShot(clip);
    }

    public void FadeBGM(AudioClip clip, float fadeOutTime, float fadeInTime)
    {
        StartCoroutine(FadeClips(clip, fadeOutTime, fadeInTime));
    }
    //brings volume to 0, changes music, brings volume to 1 - can add crossfade if necessary
    private IEnumerator FadeClips(AudioClip clip, float fadeOutTime, float fadeInTime)
    {
        if (bgmB.clip == clip) { yield break; } //guard clause

        bgmB.clip = clip;
        bgmB.Play();
        
        float lapsed = 0.0f;

        while(lapsed < fadeOutTime)
        {
            lapsed += Time.deltaTime;

            bgmA.volume = -(lapsed / fadeInTime) + 1; //volume going down
            bgmB.volume = lapsed / fadeOutTime; //volume going up

            yield return null;
        }
        bgmA.Stop();

        //We swap positions so next time this is called the procedure happens the same way
        AudioSource temp = bgmA;
        bgmA = bgmB;
        bgmB = temp;
    }

    public void RestorePreviousBGM(float fadeOutTime, float fadeInTime)
    {
        StartCoroutine(FadeClips(previousBGM, fadeOutTime, fadeInTime));
    }

    #region Internal logic
    private AudioSource FindAvailableSource() //Finds an audio source that is not currently playing anything, if there are none, creates another one
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (!sources[i].isPlaying)
                return sources[i];
        }

        sources.Add(gameObject.AddComponent<AudioSource>());
        sources[sources.Count - 1].playOnAwake = false;
        sources[sources.Count - 1].volume = 0.5f;
        sources[sources.Count - 1].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("GameAudioMixer");
        return sources[sources.Count - 1];
    }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        sources.Add(GetComponent<AudioSource>());
    }
    #endregion
}
