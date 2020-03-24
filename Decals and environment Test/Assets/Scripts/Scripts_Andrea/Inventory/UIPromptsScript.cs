using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class UIPromptsScript : MonoBehaviour
{
    Image crossHair;
    [SerializeField] Sprite interactionCrosshairSprite;
    private Sprite idleCrosshairSprite;

    [SerializeField] private TextMeshProUGUI promptTextBox;

    private void Start()
    {
        crossHair = GetComponent<Image>();
        idleCrosshairSprite = crossHair.sprite;
    }

    private void OnEnable()
    {
        FirstPersonController.FacingPromptIconEvent += ManageFade;
        FirstPersonController.FacingPromptTextEvent += SetPromptText;
    }

    private void OnDisable()
    {
        FirstPersonController.FacingPromptIconEvent -= ManageFade;
        FirstPersonController.FacingPromptTextEvent -= SetPromptText;
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
}
