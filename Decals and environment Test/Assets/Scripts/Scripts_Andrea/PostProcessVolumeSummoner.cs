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

    public static PostProcessVolumeSummoner instance;

    public enum PostProcVolumeType { TO_DARK = 0, FROM_DARK = 1 };

    private void Start()
    {
        postProcAudio = postProcBlock.GetComponent<AudioSource>();

        if (instance == null)
            instance = this;
    }

    public IEnumerator SummonVolume(PostProcVolumeType type, float timeToApproach, float peakTime, float timeToFade)
    {
        postProcAudio.Play();

        double sec = TimelineDirectorScript.instance.director.playableAsset.duration;

        yield return new WaitForSeconds((float)sec - 2.15f);
        StartCoroutine(InventoriesManager.instance.FadeFromBlackHalf(2.75f));
        //yield return new WaitForSeconds((float)sec);

        postProcAudio.Stop();
        volumeSummoning = null;
        yield return null;
    }

    bool goUp = true;
    private void Update()
    {
        /*

        if (Input.GetKeyUp(KeyCode.P) && volumeSummoning == null)
        {
            DarkTransition();
            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = false;
            GameStateManager.GetPlayer().transform.position = GameStateManager.GetPlayer().transform.position + new Vector3(0, goUp ? 10 : -10, 0);
            GameStateManager.GetPlayer().GetComponent<CharacterController>().enabled = true;
            goUp = !goUp;
        }

        */
    }

    public void DarkTransition()
    {
        volumeSummoning = StartCoroutine(SummonVolume(PostProcVolumeType.TO_DARK, 2.5f, 1.75f, 2.6f));
        TimelineDirectorScript.instance.PlaySequence(1);
    }
}