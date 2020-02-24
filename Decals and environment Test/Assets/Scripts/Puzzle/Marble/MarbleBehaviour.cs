using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleBehaviour : MonoBehaviour
{
    public float Speed;
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
        Speed = rb.velocity.magnitude * 10;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (aud.isPlaying == false && Speed >= 0.1f && collision.gameObject.tag == "Board")
        {
            Debug.Log("HittingBoard");
            aud.Play();
        }
        else
        if (aud.isPlaying == true && Speed < 0.1f && collision.gameObject.tag == "Board")
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
}
