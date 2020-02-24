using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleBehaviour : MonoBehaviour
{
    public float speed;
    public float speedModifier = 10;
    AudioSource aud;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }



    private void FixedUpdate()
    {
        speed = rb.velocity.magnitude * speedModifier;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (aud.isPlaying == false && speed >= 0.1f && collision.gameObject.tag == "Board")
        {
            Debug.Log("HittingBoard");
            aud.Play();
        }
        else
        if (aud.isPlaying == true && speed < 0.1f && collision.gameObject.tag == "Board")
        {
            aud.Pause();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (aud.isPlaying == true && collision.gameObject.tag == "Board")
        {
            aud.Pause();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        speedModifier = 0;
    }
}
