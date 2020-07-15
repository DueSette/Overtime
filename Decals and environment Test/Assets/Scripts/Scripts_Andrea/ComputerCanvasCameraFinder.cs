using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCanvasCameraFinder : MonoBehaviour
{
    static Camera dynamicCamera;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dynamicCamera == null)
        {
            dynamicCamera = GameObject.FindGameObjectWithTag("DynamicCamera").GetComponent<Camera>();

            GetComponent<Canvas>().worldCamera = dynamicCamera;
        }
    }
}
