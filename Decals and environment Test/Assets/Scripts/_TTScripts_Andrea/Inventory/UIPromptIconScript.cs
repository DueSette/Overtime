using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPromptIconScript : MonoBehaviour
{
    Image image;
    [SerializeField, Range(0.1f, 3)] float fadeSpeed = 1;
    float alpha = 0;


    private void Start()
    {
        image = GetComponent<Image>();
    }   

    public void ManageFade(bool up)
    {
        alpha = Mathf.Clamp(alpha + (up ? 0.025f : -0.025f) * fadeSpeed, 0, 1);
        Color c = new Color(255, 255, 255, alpha);
        image.color = c;
    }
}
