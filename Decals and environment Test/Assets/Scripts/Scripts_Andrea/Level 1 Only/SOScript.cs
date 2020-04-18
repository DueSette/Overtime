using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOScript : MonoBehaviour
{
    [SerializeField] private Animator anim;

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
        if (Vector3.Distance(playerPos, transform.position) < 5)
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

        // Rotating SO
        // Moving SO
        yield return null;


        // SO Is No Longer Needed
        this.enabled = false;
    }


    /*
    ====================================================================================================
    Utility
    ====================================================================================================
    */
    private IEnumerator LerpTowardPosition(Vector3 startPos, Vector3 endPos, float time, LerpType lerpType)
    {
        float t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime / time);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, CalculateLerpType(t, lerpType));
            this.transform.position = newPos;

            yield return null;
        }

        this.transform.position = endPos;
    }

    private IEnumerator LerpTowardRotation(Vector3 startRot, Vector3 endRot, float time, LerpType lerpType)
    {
        float t = 0;
        while (t <= 1)
        {
            t += (Time.deltaTime / time);
            Vector3 newRot = Vector3.Lerp(startRot, endRot, CalculateLerpType(t, lerpType));
            this.transform.eulerAngles = newRot;

            yield return null;
        }

        this.transform.eulerAngles = endRot;
    }

    private float CalculateLerpType(float lerpTime, LerpType type)
    {
        float adjustedTime = 0;

        switch (type)
        {
            case (LerpType.LINEAR):
                {
                    adjustedTime = lerpTime;
                    return adjustedTime;
                }

            case (LerpType.ACCELERATED):
                {
                    adjustedTime = (lerpTime * lerpTime);
                    return adjustedTime;
                }

            case (LerpType.DECELERATED):
                {
                    adjustedTime = (1 - (1 - lerpTime) * (1 - lerpTime));
                    return adjustedTime;
                }

            case (LerpType.SMOOTHSTEP):
                {
                    float stepStart = (lerpTime * lerpTime);
                    float stepEnd = (1 - (1 - lerpTime) * (1 - lerpTime));

                    adjustedTime = Mathf.Lerp(stepStart, stepEnd, lerpTime);
                    return adjustedTime;
                }

            default:
                {
                    adjustedTime = lerpTime;
                    return adjustedTime;
                }
        }
    }

    private enum LerpType
    {
        LINEAR,
        ACCELERATED,
        DECELERATED,
        SMOOTHSTEP
    };
}
