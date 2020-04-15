using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidScript : MonoBehaviour
{
    // VERY QUICK AND DIRTY THING DONT JUDGE ME
    // TODO: ALL THE JUDGEMENT WILL BE GIVEN
    // TODO: </3

    [SerializeField]
    private GameObject candle;

    [SerializeField]
    private Transform candleSpot;

    [SerializeField]
    private Animator anim;

    int eventNumber = 0;
    bool canTrigger = true;


    /*
    ====================================================================================================
    Handling/ Triggering Hallway Sequence Events
    ====================================================================================================
    */
    void Update()
    {
        // Starts the sequence once the player is close enough to the gameobject
        if (IsPlayerNear())
        {
            TriggerNextEvent();
        }
    }


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
            switch (eventNumber)
            {
                case (0):
                    {
                        StartCoroutine(RunDownCorridor());
                    }
                    break;

                case (1):
                    {
                        StartCoroutine(RunIntoDoor());
                    }
                    break;
            }
        }
    }


    /*
    ====================================================================================================
    Sequence Events
    ====================================================================================================
    */
    private IEnumerator RunDownCorridor()
    {
        // Start Event
        canTrigger = false;

        //StartCoroutine(LerpTowardRotation(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 180.0f, 0.0f), 1.0f)); // Rotation Handled By Animations
        anim.SetTrigger("approached1");
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(LerpTowardPosition(this.transform.position, new Vector3(1.2f, 0.0f, -4.0f), 3.0f, LerpType.ACCELERATED));
        yield return new WaitForSeconds(3.0f);

        //StartCoroutine(LerpTowardRotation(new Vector3(0.0f, 180.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), 1.0f, LerpType.LINEAR));
        anim.SetTrigger("idle");
        yield return new WaitForSeconds(1.0f);


        // End Event
        eventNumber++;
        canTrigger = true;
    }

    private IEnumerator RunIntoDoor()
    {
        // Start Event
        canTrigger = false;


        StartCoroutine(LerpTowardRotation(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 60.0f, 0.0f), 0.25f, LerpType.LINEAR));
        StartCoroutine(LerpTowardPosition(this.transform.position, new Vector3(3f, 0.0f, -4.0f), 0.75f, LerpType.ACCELERATED));
        anim.SetTrigger("approached2");
        yield return new WaitForSeconds(0.5f);

        candle.transform.SetParent(null);
        candle.transform.position = candleSpot.position;
        candle.GetComponent<Rigidbody>().useGravity = true;
        candle.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);


        // End Event
        eventNumber++;
        canTrigger = true;
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
