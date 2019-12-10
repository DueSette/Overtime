using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    Light testLight;
    public float minWaitTime;
    public float maxWaitTime;
    public GameObject slender;
    public AudioSource audio;
    public AudioClip clip;

    void Start()
    {
        testLight = GetComponent<Light>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            testLight.enabled = !testLight.enabled;
            yield return new WaitForSeconds(minWaitTime);
            testLight.enabled = !testLight.enabled;

        }
    }

    private void Update()
    {
        if (testLight.isActiveAndEnabled)
        {
            slender.SetActive(false);
        }
        else
        {
            slender.SetActive(true);
            audio.Play();
        }
    }
}
