using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
public enum GameState { IN_GAME, MENU, CUTSCENE, PAUSE }

public class GameStateManager : MonoBehaviour
{ 
    public static GameState gameState { get; private set; } = GameState.IN_GAME;
    private static GameState previousState;
    private static GameObject player;

    private void Start()
    {
        player = FindObjectOfType<FirstPersonController>().gameObject;
    }

    public static void UpdateGameState(GameState gs)
    {
        previousState = gameState;
        gameState = gs;
        print("Game State changed to: " + gs);

        ReactToStateChange();
    }

    public static void RestorePreviousState()
    {
        gameState = previousState;
        print("Game State restored to previous state: " + previousState);
    }

    private static void ReactToStateChange()
    {
        switch(gameState)
        {
            case GameState.IN_GAME:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case GameState.MENU:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            default:
                break;
        }

        print("Cursor state: " + Cursor.lockState);
    }

    public static GameObject GetPlayer() { return player; }
}
