using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessVolumeSummoner : MonoBehaviour
{
    //This class manages everything related to post process effects, including ppVolumes, timeline, and specific game objects

    [SerializeField, Tooltip("This is the FPS controller's child, its transform is used to move the Volumes around")]
    GameObject postProcBlock;
    [SerializeField, Tooltip("These need to be put in order: Transition to Dark, from Dark, etc")]
    VolumeProfile[] profiles;

    Coroutine volumeSummoning;
    AudioSource postProcAudio;

    public enum PostProcVolumeType { TO_DARK = 0, FROM_DARK = 1 };

    private void Start()
    {
        postProcAudio = postProcBlock.GetComponent<AudioSource>();
    }

    public IEnumerator SummonVolume(PostProcVolumeType type, float timeToApproach, float peakTime, float timeToFade)
    {
        postProcAudio.Play();

        double sec = TimelineDirectorScript.instance.director.playableAsset.duration;
        yield return new WaitForSeconds((float)sec);

        postProcAudio.Stop();
        volumeSummoning = null;
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && volumeSummoning == null)
        {
            volumeSummoning = StartCoroutine(SummonVolume(PostProcVolumeType.TO_DARK, 2.5f, 1.75f, 2.6f));
            TimelineDirectorScript.instance.PlaySequence(1);
        }
    }
}