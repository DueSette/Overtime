using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScript : MonoBehaviour, IInteractable, ITextPrompt
{
    public delegate void ParentInteraction();
    public static event ParentInteraction OnParentInteraction;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void IInteractable.InteractWith()
    {
        CauseReaction();
        OnParentInteraction();
    }

    string ITextPrompt.PromptText()
    {
        return "Maybe mom can help me light the candle";
    }

    void CauseReaction()
    {
        int r = Random.Range(0, 2);
        if (r == 0)        
            anim.SetTrigger("Agony");       
        else
            anim.SetTrigger("Disbelief");
    }
}
