using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelButton : MonoBehaviour, IInteractable, ITextPrompt
{
    void IInteractable.InteractWith()
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameController");

        LevelManager lm = g.GetComponent<LevelManager>();
        lm.NextLevel();
    }

    string ITextPrompt.PromptText()
    {
        return "Time to move to the next level...";
    }
}
