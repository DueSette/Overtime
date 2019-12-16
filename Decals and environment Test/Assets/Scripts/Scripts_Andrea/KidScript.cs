using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidScript : MonoBehaviour
{
    //VERY QUICK AND DIRTY THING DONT JUDGE ME

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

    void IsPlayerNear()
    {
        Vector3 pPos = GameStateManager.GetPlayer().transform.position;

        if (Vector3.Distance(pPos, transform.position) < 8)
        {
            once = false;
            anim.SetTrigger("approached");
        }

        StartCoroutine(EnableSpeed());
    }

    IEnumerator EnableSpeed()
    {
        yield return new WaitForSeconds(0.85f);
        speed = 2;
    }
}
