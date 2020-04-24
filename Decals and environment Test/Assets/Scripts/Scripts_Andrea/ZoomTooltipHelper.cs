using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomTooltipHelper : MonoBehaviour
{
    [SerializeField] Image tooltip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("FPS"))
            StartCoroutine(FadeTooltip());
    }

    IEnumerator FadeTooltip()
    {
        float lapsed = 0;
        Color startColor = tooltip.color;

        while(lapsed < 1.5f)
        {
            lapsed += Time.deltaTime;
            tooltip.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), lapsed / 1.5f);
            yield return null;
        }

    }
}
