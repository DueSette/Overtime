using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class CrosshairScript : MonoBehaviour
{
    Image crossHair;
    [SerializeField] private Sprite interactionCrosshairSprite;
    private Sprite idleCrosshairSprite;

    [SerializeField] private TextMeshProUGUI promptTextBox;

    private void Awake()
    {
        crossHair = GetComponent<Image>();
        idleCrosshairSprite = crossHair.sprite;
    }

    private void OnEnable()
    {
        FirstPersonController.FacingPromptIconEvent += ManageFade;
        FirstPersonController.FacingPromptTextEvent += SetPromptText;
        GameStateManager.OnStateChange += SetCrosshairVisibility;
    }

    private void OnDisable()
    {
        FirstPersonController.FacingPromptIconEvent -= ManageFade;
        FirstPersonController.FacingPromptTextEvent -= SetPromptText;
        GameStateManager.OnStateChange -= SetCrosshairVisibility;
    }

    private void ManageFade(bool facingInteractable)
    {
        if (crossHair == null) { return; }

        crossHair.sprite = facingInteractable ? interactionCrosshairSprite : idleCrosshairSprite;
    }

    private void SetPromptText(string s)
    {
        promptTextBox.SetText(s);
    }

    void SetCrosshairVisibility(GameState gs)
    {
        crossHair.enabled = gs == (GameState.CAMERA_FOCUS & GameState.IN_GAME_LOOK_ONLY & GameState.IN_GAME & GameState.INTERACTING_W_ITEM & GameState.IN_GAME_LOOK_ONLY);
    }
}
