using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public enum GameState
{   IN_GAME = 0,
    MENU = 2,
    CUTSCENE = 4,
    PAUSE = 8,
    IN_GAME_LOOK_ONLY = 16, //this one is for specific cases where you can move the camera but can't walk
    INTERACTING_W_ITEM = 32,
    CAMERA_FOCUS = 64
}

public class GameStateManager : MonoBehaviour
{
    public static GameState gameState { get; private set; } = GameState.IN_GAME;
    private static GameState previousState;
    private static GameObject player;

    public delegate void StateChangeDelegate(GameState gs);
    public static event StateChangeDelegate OnStateChange;

    private void Awake()
    {
        player = FindObjectOfType<FirstPersonController>().gameObject;
    }

    public static void SetGameState(GameState gs)
    {
        previousState = gameState;
        gameState = gs;
        print("Game State changed to: " + gs);

        HandleStateChange();
    }

    public static void RestorePreviousState()
    {
        GameState temp = gameState; //need it so we can cache the new previous state

        gameState = previousState;
        previousState = temp;

        HandleStateChange();
        print("Game State restored to previous state: " + previousState);
    }

    private static void HandleStateChange() //whenever state is changed, do stuff according to the new state
    {
        switch (gameState)
        {
            case GameState.IN_GAME:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.MENU:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case GameState.CUTSCENE:
                break;

            case GameState.CAMERA_FOCUS:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case GameState.INTERACTING_W_ITEM:               
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            default:
                break;
        }
        OnStateChange(gameState);
    }

    public static GameObject GetPlayer() { return player; }
}