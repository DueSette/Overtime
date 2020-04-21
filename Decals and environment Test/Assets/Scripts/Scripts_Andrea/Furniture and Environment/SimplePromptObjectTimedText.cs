using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Point of this class is to be used for anything that populates the prompt text box in the bottom center part of the UI
 */

public class SimplePromptObjectTimedText : MonoBehaviour, ITextPrompt
{
    [SerializeField] string promptText;

    [SerializeField, Tooltip("Should the sound just linger on the screen the first time the object is hovered and then disappear?")]
    public float lingerTime;

    string ITextPrompt.PromptText()
    {
        Destroy(this);
        return promptText;
    }
}
