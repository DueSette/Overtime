using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TravelDirection directionOfMovement;
    [SerializeField] private Transform triggerPoint;
    [SerializeField] private float timeBeforeFade;
    [SerializeField] private float timeForFade;
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color fadedColor;

    bool canTrigger = true;

    // Update is called once per frame
    void Update()
    {
        // Starts the sequence once the player is close enough to the gameobject
        if (IsPlayerNear())
        {
            TriggerNextEvent();
        }
    }


    /*
    ====================================================================================================
    Handling/ Triggering Hallway Sequence Events
    ====================================================================================================
    */
    bool IsPlayerNear()
    {
        Vector3 playerPos = GameStateManager.GetPlayer().transform.position;
        if (Vector3.Distance(playerPos, triggerPoint.transform.position) < 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TriggerNextEvent()
    {
        if (canTrigger)
        {
            StartCoroutine(WalkToSpot());
        }
    }


    /*
    ====================================================================================================
    Sequence Events
    ====================================================================================================
    */
    IEnumerator WalkToSpot()
    {
        // Event Start
        canTrigger = false;

        // Movement Animation
        switch (directionOfMovement)
        {
            case (TravelDirection.LEFT):
                anim.SetTrigger("TurnLeft");
                break;

            case (TravelDirection.RIGHT):
                anim.SetTrigger("TurnRight");
                break;
        }

        yield return new WaitForSeconds(timeBeforeFade);

        // Fade Animation
        float t = 0;
        while (t < 1.0f)
        {
            t += (Time.deltaTime / timeForFade);

            Color newColor = Color.Lerp(defaultColor, fadedColor, t);

            fadeMaterial.SetColor("_TintColor", newColor);

            yield return null;
        }


        // SO Is No Longer Needed
        anim.gameObject.SetActive(false);
        // Resetting Material Visibility
        fadeMaterial.SetColor("_TintColor", defaultColor);
    }

    private enum TravelDirection
    {
        RIGHT,
        LEFT
    };
}
