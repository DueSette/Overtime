﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelButton : MonoBehaviour, IInteractable, ITextPrompt
{
    void IInteractable.InteractWith()
    {
        LevelManager.onLevelEvent("EndLevel");
    }

    string ITextPrompt.PromptText()
    {
        return "It seems this only goes one way...";
    }
}
