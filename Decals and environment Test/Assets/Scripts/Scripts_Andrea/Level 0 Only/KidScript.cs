using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidScript : MonoBehaviour
{
    // VERY QUICK AND DIRTY THING DONT JUDGE ME
    // TODO: ALL THE JUDGEMENT WILL BE GIVEN

    [SerializeField] GameObject candle;
    float speed = 0;
    Animator anim;
    bool once = true;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(once)
            IsPlayerNear();
        if (!once)
            transform.Translate(-Vector3.forward * Time.deltaTime * speed);
    }


    /*
    ====================================================================================================
    Handling/ Triggering Hallway Sequence Events
    ====================================================================================================
    */
    void IsPlayerNear()
    {
        Vector3 pPos = GameStateManager.GetPlayer().transform.position;

        if (Vector3.Distance(pPos, transform.position) < 8)
        {
            once = false;
            anim.SetTrigger("approached");
            StartCoroutine(EnableSpeed());
        }      
    }


    /*
    ====================================================================================================
    Event Logic
    ====================================================================================================
    */
    // Event 1 - Running down the corridor, away from the player
    IEnumerator EnableSpeed()
    {
        yield return new WaitForSeconds(0.85f);
        speed = 4.2f;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    // Event 2 - Turning and running through the door
}
