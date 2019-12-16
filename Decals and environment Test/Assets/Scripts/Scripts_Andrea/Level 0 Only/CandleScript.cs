using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    Rigidbody rb;
    AudioSource aud;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().size /= 2.5f;
        rb.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        aud.Play();
        transform.parent = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        aud.Play();
    }
}
