using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmailOnClickUtility : MonoBehaviour
{
    //This component needs to be attached to every Email UI GameObject. The methods within will be called via button click.
    public void ClearEmailTextStyles()
    {
        gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
