using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    Rigidbody rb;
    AudioSource aud;
    [SerializeField] GameObject cake;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        aud.Play();
        transform.parent = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        aud.Play();
    }

    private void OnDestroy()
    {
        cake.SetActive(true);
    }
}
