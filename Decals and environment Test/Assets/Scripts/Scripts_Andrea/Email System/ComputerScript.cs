using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerScript : MonoBehaviour, IInteractable
{
    [SerializeField] List<EmailScriptableObject> containedEmails = new List<EmailScriptableObject>();
    [SerializeField] GameObject emailEntryPrefab;
    [SerializeField] GameObject rightPanelDisplay;
    bool beingInteractedWith = false;

    private void OnEnable()
    {
        //possible setup logic
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += LeaveInteraction;
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= LeaveInteraction;
    }
    private void Update()
    {
        if (beingInteractedWith)
            CheckInput();
    }

    void CheckInput()
    {
        //navigation stuff: up arrow, down arrow, onClick
    }

    public void OnEmailClick(EmailScriptableObject email)
    {
        //update right panel with the data found within the clicked email
    }

    void IInteractable.InteractWith()
    {
        beingInteractedWith = true;
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);

        //go from standbyscreen to displaying stuff
        //load entries, keep right hand panel blank
        //should probably make mouse visible and unlocked
        //tell camera to do something maybe
    }

    public void LeaveInteraction()
    {
        GameStateManager.SetGameState(GameState.IN_GAME);

    }
}
