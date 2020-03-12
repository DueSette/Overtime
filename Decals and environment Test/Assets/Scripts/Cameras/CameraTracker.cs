using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public GameObject theFPSCamera;

    private void Start()
    {
        transform.position = theFPSCamera.transform.position;
        transform.rotation = theFPSCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eulerRotation = new Vector3(theFPSCamera.transform.eulerAngles.x, 
                                            theFPSCamera.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
