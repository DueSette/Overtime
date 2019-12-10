using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDirectorScript : MonoBehaviour
{
    PlayableDirector director;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        director.played += OnPlayableDirectorPlayed;
        director.stopped += OnPlayableDirectorStopped;

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
             GameStateManager.UpdateGameState(GameState.CUTSCENE);
    }

    void OnDisable()
    {
        director.played -= OnPlayableDirectorPlayed;
    }
}
