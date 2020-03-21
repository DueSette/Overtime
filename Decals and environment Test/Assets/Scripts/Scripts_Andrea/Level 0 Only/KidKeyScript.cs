using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KidKeyScript : MonoBehaviour
{
    AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        aud.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        aud.Play();
    }
}
