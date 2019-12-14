using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDirectorScript : MonoBehaviour
{
    [HideInInspector] public PlayableDirector director;
    public static TimelineDirectorScript instance;
    public PlayableAsset[] sequences;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void OnEnable()
    {
        director = GetComponent<PlayableDirector>();
        director.played += OnPlayableDirectorPlayed;
        director.stopped += OnPlayableDirectorStopped;

        director.playableAsset = sequences[0];
        director.Play();
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
            GameStateManager.RestorePreviousState();
    }

    void OnPlayableDirectorPlayed(PlayableDirector aDirector)
    {
        if (director == aDirector)
             GameStateManager.SetGameState(GameState.CUTSCENE);
    }

    void OnDisable()
    {
        director.played -= OnPlayableDirectorPlayed;
        director.stopped -= OnPlayableDirectorStopped;
    }

    public void PlaySequence(int whichSequence)
    {
        director.playableAsset = sequences[whichSequence];
        director.Play();
    }
}
