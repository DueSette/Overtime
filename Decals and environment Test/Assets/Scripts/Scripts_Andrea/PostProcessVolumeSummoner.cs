using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessVolumeSummoner : MonoBehaviour
{
    /*
     * 
     * REQUIREMENTS
     * Choose a Volume profile from list
     * Equip it to deisgnated GameObject
     * Choose speed, proximity, lingering (how long to stay at a certain proximity)
     */

    [SerializeField, Tooltip("This is the FPS controller's child, its transform is used to move the Volumes around")]
    GameObject postProcBlock;
    [SerializeField, Tooltip("These need to be put in order: Transition to Dark, from Dark, etc")]
    VolumeProfile[] profiles;
    Volume equippedVolume;

    Vector3 startingRelativePos; //the initial point of the block
    AudioSource postProcAudio;
    public enum PostProcVolumeType { TO_DARK = 0, FROM_DARK = 1 };

    private void Start()
    {
        equippedVolume = postProcBlock.GetComponent<Volume>();
        postProcAudio = postProcBlock.GetComponent<AudioSource>();
        startingRelativePos = postProcBlock.transform.localPosition;
    }

    public IEnumerator SummonVolume(PostProcVolumeType type, float timeToApproach, float peakTime, float timeToFade)
    {
        equippedVolume.profile = profiles[(int)type];
        postProcAudio.Play();

        GameStateManager.SetGameState(GameState.CUTSCENE);

        yield return StartCoroutine(MoveToCharacter(timeToApproach));
        yield return new WaitForSeconds(peakTime);
        yield return MoveAwayFromCharacter(timeToFade);

        GameStateManager.SetGameState(GameState.IN_GAME);

        equippedVolume.profile = null;
        postProcAudio.Stop();
        yield return null;
    }

    private IEnumerator MoveToCharacter(float timeApproach)
    {
        float lap = 0.0f;

        while(lap < timeApproach)
        {
            postProcBlock.transform.localPosition = Vector3.Lerp(startingRelativePos, Vector3.zero, lap / timeApproach);
            lap += Time.deltaTime;

            if (lap > timeApproach)
                lap = timeApproach;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator MoveAwayFromCharacter(float timeFade)
    {
        float lap = 0.0f;
        Vector3 currPos = postProcBlock.transform.localPosition;

        while (lap < timeFade)
        {
            postProcBlock.transform.localPosition = Vector3.Lerp(currPos, startingRelativePos, lap / timeFade);
            lap += Time.deltaTime;

            if (lap > timeFade)
                lap = timeFade;
            yield return null;
        }

        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            StartCoroutine(SummonVolume(PostProcVolumeType.TO_DARK, 2.5f, 1.75f, 2.6f));
            TimelineDirectorScript.instance.PlaySequence(1);
        }
    }
}
