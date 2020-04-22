using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCanvasCameraFinder : MonoBehaviour
{
    static Camera camera;
    void Start()
    {
        if(camera == null)
            camera = GameObject.FindGameObjectWithTag("DynamicCamera").GetComponent<Camera>();

        GetComponent<Canvas>().worldCamera = camera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
