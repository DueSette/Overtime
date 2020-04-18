using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmailClickUtility : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public delegate void DeselectDelegate();
    public static DeselectDelegate EmailDeselectEvent;
    static ComputerScript computer;

    private void Start()
    {
        if(computer == null)
            computer = GetComponentInParent<ComputerScript>();
    }

    public void ClearEmailTextStyles()
    {
        gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData) //so the system can be aware of when we have no emails selected
    {
        EmailDeselectEvent();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        computer.DisplayEmailOnClick(GetComponentInChildren<TextMeshProUGUI>());
    }
}
