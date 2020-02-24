using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject thefpsController;
    public GameObject theCharacter;
    public GameObject theBoard;
    public Camera playerCamera;
    public Camera marbleCamera;
    public CharacterController controller;


    AudioListener playerCamAud;
    AudioListener marbleCamAud;


    public int cameraNum;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = theCharacter.GetComponent<Camera>();
        marbleCamera = theBoard.GetComponent<Camera>();
        controller = thefpsController.GetComponent<CharacterController>();

        

        playerCamAud = thefpsController.GetComponent<AudioListener>();

        marbleCamAud = theBoard.GetComponent<AudioListener>();

        cameraNum = 1;
    }

    // Update is called once per frame
    void Update()
    {

        cameraNum = BoardScript.camNum;


        if (cameraNum == 1)
        {
            playerCamera.enabled = true;
            playerCamAud.enabled = true;
            controller.enabled = true;
            marbleCamera.enabled = false;
            marbleCamAud.enabled = false;
        }

        

        if (cameraNum == 2)
        {
            marbleCamera.enabled = true;
            marbleCamAud.enabled = true;
            controller.enabled = false;
            playerCamera.enabled = false;
            playerCamAud.enabled = false;
        }

    }


    void CameraChange()
    {
        if (cameraNum == 1)
        {
            playerCamera.enabled = true;
            playerCamAud.enabled = true;
            marbleCamera.enabled = false;
            marbleCamAud.enabled = false;
        }

        if (cameraNum == 2)
        {
            marbleCamera.enabled = true;
            marbleCamAud.enabled = true;
            playerCamera.enabled = false;
            playerCamAud.enabled = false;
        }
    }
}
