using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOfInterest : MonoBehaviour
{
    public DynamicCamera theCamera;
    public CameraSwitch cameraController;
    public int checker = 1;

    /// <summary>
    /// Attatch this script to the Parent Gameobject you want the player to interact with.
    /// 
    /// The gameobject must have a child gameobject attatched with the "Viewport" tag.
    /// 
    /// MAKE SURE DYNAMIC CAMERA IS TURNED ON IN EDITOR BEFORE GAME STARTS
    /// </summary>




    void Start()
    {
       // cameraController = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraSwitch>();
    }

    public virtual void FocusCamera()
    {
        cameraController = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraSwitch>();
        cameraController.CameraChange();
        Invoke("FindCamera", .2f);
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
        theCamera.viewNum = 1; // Starts the camera at viewpoint everytime it's clicked.
    }        
}
