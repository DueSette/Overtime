using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerScript : MonoBehaviour, IInteractable
{
    [SerializeField] ComputerSystem computerSystem;
    [SerializeField] Image spaceTooltip;

    [SerializeField] GameObject standbyScreen;
    private Material screensaverMat;

    [SerializeField] AudioClip logon, logoff;
    bool beingInteractedWith = false;

    #region Unity methods
    private void OnEnable()
    {
        //possible setup logic
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += LeaveInteraction;
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= LeaveInteraction;
    }

    private void Start()
    {
        screensaverMat = standbyScreen.GetComponent<Image>().material;
        screensaverMat.SetFloat("_Fade", 0);
        screensaverMat.SetFloat("_Transition", 0);
    }

    private void Update() //quick and dirty scrolling sound logic
    {
        if(beingInteractedWith == false)
        {
            return;
        }
        else
        {
            computerSystem.UpdateSystem();
        }
    }
    #endregion

    

    #region Interaction related methods
    void IInteractable.InteractWith() //removes standby screen, wakes up cursor
    {
        StopAllCoroutines();
        StartCoroutine(InteractAnim());
    }

    private IEnumerator InteractAnim()
    {
        beingInteractedWith = true;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        spaceTooltip.enabled = true;
        SoundManager.instance.PlaySound(logon);

        computerSystem.StartupSystem();

        float t;
        // Fade Animation
        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / 0.5f);

            screensaverMat.SetFloat("_Fade", Mathf.Lerp(0, 1, t));

            yield return null;
        }

        // Small Wait
        yield return new WaitForSeconds(0.2f);

        // Transition Animation
        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / 1.0f);

            screensaverMat.SetFloat("_Transition", Mathf.Lerp(0, 100, t));

            yield return null;
        }
        
        standbyScreen.SetActive(false);
    }

    public void LeaveInteraction() //reverts the computer back to how it was before being interacted with
    {
        if(!beingInteractedWith)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(LeaveAnim());
    }

    private IEnumerator LeaveAnim()
    {
        standbyScreen.SetActive(true);

        spaceTooltip.enabled = false;
        SoundManager.instance.PlaySound(logoff);

        float t;

        // Transition Animation
        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / 0.8f);

            screensaverMat.SetFloat("_Transition", Mathf.Lerp(100, 0, t));

            yield return null;
        }

        // Small Wait
        yield return new WaitForSeconds(0.1f);

        // Fade Animation
        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / 0.2f);

            screensaverMat.SetFloat("_Fade", Mathf.Lerp(1, 0, t));

            yield return null;
        }

        beingInteractedWith = false;
    }

        #endregion
    }
