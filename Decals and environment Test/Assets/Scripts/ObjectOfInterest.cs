using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOfInterest : MonoBehaviour
{
    private DynamicCamera theCamera;
    private CameraSwitch cameraController;
    public int checker = 1;

    void Start()
    {
        cameraController = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraSwitch>();
    }

    void Update()
    {
       
    }

    public void FocusCamera()
    {
        cameraController.CameraChange();
        Invoke("FindCamera", 0.2f);       
    }

    void FindCamera()
    {
        Debug.Log("findcamera called");
        theCamera = GameObject.FindGameObjectWithTag("DynamicCamera").GetComponent<DynamicCamera>();

        foreach (Transform child in transform)
        {
            if (child.tag == "ViewPort")
            {
                Debug.Log("child found");
                theCamera.viewList.Add(child);
            }
        }
    }        
}
