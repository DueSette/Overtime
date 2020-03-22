using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Point of this class is to be used for anything that populates the prompt text box in the bottom center part of the UI
 */

public class SimplePromptObjectTextOnly : MonoBehaviour, ITextPrompt
{
    [SerializeField] string promptText;

    string ITextPrompt.PromptText()
    {
        return promptText;
    }
}
