using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Point of this class is to be used for anything that "prompts" the player, that is it interacts with the player's passive and/or active raycasting
 * This class is to be used only with objects that have behaviours so limited that we don't need to give them a dedicated class
 */

public class SimplePromptObject : MonoBehaviour, IInteractable, ITextPrompt
{
    [SerializeField] AudioClip clip;
    [SerializeField] string promptText;

    void IInteractable.InteractWith()
    {
        SoundManager.instance.PlaySound(clip);
    }

    string ITextPrompt.PromptText()
    {
        return promptText;
    }
}
