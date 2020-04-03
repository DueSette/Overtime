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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            GameStateManager.SetGameState(GameState.IN_GAME_LOOK_ONLY);
    }

    void OnEnable()
    {
        director = GetComponent<PlayableDirector>();
        director.played += OnPlayableDirectorPlayed; //these events exist by default, we are just subscribing new methods to them
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Start()
    {
        director.playableAsset = sequences[0];
        director.Play();
    }

    //if the director that fired the event is this director, act upon it
    void OnPlayableDirectorPlayed(PlayableDirector aDirector)
    {
        if (director == aDirector)
            GameStateManager.SetGameState(GameState.CUTSCENE);
    }

    //if the director that fired the event is this director, act upon it
    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
            GameStateManager.RestorePreviousState();
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