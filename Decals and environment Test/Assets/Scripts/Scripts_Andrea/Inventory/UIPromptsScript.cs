using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class UIPromptsScript : MonoBehaviour
{
    Image image;
    [SerializeField, Range(0.1f, 3)] float fadeSpeed = 1;
    [SerializeField] private TextMeshProUGUI promptTextBox;
    float alpha = 0;

    private void Start()
    {
        image = GetComponent<Image>();
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

    private void ManageFade(bool up)
    {
        if (image == null)
            return;

        alpha = Mathf.Clamp(alpha + (up ? 0.025f : -0.025f) * fadeSpeed, 0, 1);
        Color c = new Color(255, 255, 255, alpha);
        image.color = c;
    }

    private void SetPromptText(string s)
    {
        promptTextBox.SetText(s);
    }
}
