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
        rb.AddForce(Vector3.forward * 10, ForceMode.Impulse);
        aud.Play();
        transform.parent = null;
        rb.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        aud.Play();
    }
}
