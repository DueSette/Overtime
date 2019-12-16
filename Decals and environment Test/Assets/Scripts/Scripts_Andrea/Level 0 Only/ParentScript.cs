using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScript : MonoBehaviour, IInteractable, ITextPrompt
{
    public delegate void ParentInteraction();
    public static event ParentInteraction OnParentInteraction;

    void IInteractable.InteractWith()
    {
        OnParentInteraction();
    }

    string ITextPrompt.PromptText()
    {
        return "Maybe mom and dad can light the candle";
    }
}
