using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimatorScript : MonoBehaviour
{
    [SerializeField] private GameObject hoursHand, minutesHand, secondsHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the time of the system clock
        System.DateTime currentTime = System.DateTime.Now;

        // Separating into hours, mins & seconds
        float hour = currentTime.Hour;
        float minute = currentTime.Minute;
        float second = currentTime.Second;

        // Reflecting current time in model's hands
        Vector3 newRot = new Vector3();
        // Hours
        newRot.z = (360 / 12) * (hour + (minute / 60));
        hoursHand.transform.localEulerAngles = newRot;

        // Minutes
        newRot.z = (360 / 60) * (minute + (second / 60));
        minutesHand.transform.localEulerAngles = newRot;

        // Seconds
        newRot.z = (360 / 60) * (second);
        secondsHand.transform.localEulerAngles = newRot;
    }
}
