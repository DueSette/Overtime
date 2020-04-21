﻿using System.Collections;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject thefpsController;
    public GameObject theCharacter;
    public GameObject dynamicCameraGameObj;
    public GameObject dynamicCameraHolder;
    public Camera playerCamera;
    public Camera dynamicCamera;
    public CharacterController controller;
    public DynamicCamera dynCamScript;
    public bool camerafocus;

    public static bool isCameraMain = true;

    AudioListener playerCamAud;
    AudioListener dynamicCamAud;


    public int cameraNum;

    // Start is called before the first frame update
    void Start()
    {
       // dynamicCameraHolder = GameObject.FindGameObjectWithTag("DynamicCameraHolder");
        //dynamicCameraGameObj = dynamicCameraHolder.transform.GetChild(0).gameObject;
        theCharacter = GameObject.FindGameObjectWithTag("MainCamera");
        thefpsController = theCharacter.transform.parent.gameObject;
        playerCamera = theCharacter.GetComponent<Camera>();       
        dynamicCamera = dynamicCameraGameObj.GetComponent<Camera>();
        controller = thefpsController.GetComponent<CharacterController>();
        playerCamAud = thefpsController.GetComponent<AudioListener>();
        dynamicCamAud = dynamicCameraGameObj.GetComponent<AudioListener>();
        dynCamScript = dynamicCameraGameObj.GetComponent<DynamicCamera>();

        cameraNum = 1;

        dynamicCamAud.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        // cameraNum = BoardScript.camNum;
        if (Input.GetKey(KeyCode.Space) && camerafocus == true)
        {
            camerafocus = false;
            dynCamScript.viewList.RemoveRange(1, dynCamScript.viewList.Count - 1);
            dynCamScript.viewNum = 0;
            StartCoroutine(CameraDelay());
        }

        if (Input.GetKey(KeyCode.K))
        {
            cameraNum = 2;
        }


        if (cameraNum == 1)
        {
            // Use the main fps controller and main camera
            thefpsController.SetActive(true);           
            playerCamera.enabled = true;
            playerCamAud.enabled = true;
            controller.enabled = true;

            // Hiding The Dynamic Camera
            dynamicCameraHolder.SetActive(false);
            dynamicCamera.enabled = false;
            dynamicCamAud.enabled = false;
            dynamicCameraGameObj.transform.parent = dynamicCameraHolder.transform;
            CameraSwitch.isCameraMain = true;
        }

        
        if (cameraNum == 2)
        {
            // Use he dynamic camera
            dynamicCameraHolder.SetActive(true);
            dynamicCamera.enabled = true;
            dynamicCamAud.enabled = true;
            dynamicCameraGameObj.transform.parent = null;


            // Hide the main fps controller and main camera
            //thefpsController.GetComponent<Camera>().enabled = false;
            dynamicCameraGameObj.transform.parent = null;
            controller.enabled = false;
            playerCamera.enabled = false;
            playerCamAud.enabled = false;
            CameraSwitch.isCameraMain = false;
        }

    }


    IEnumerator CameraDelay()
    {
        yield return new WaitForSeconds(0.15f);
        GameStateManager.SetGameState(GameState.IN_GAME);
        cameraNum = 1;
    }


    public void CameraChange()
    {
        //GameStateManager.SetGameState(GameState.CAMERA_FOCUS);
        camerafocus = true;
        cameraNum = 2;
    }

    public void CameraChangeToMain()
    {
        cameraNum = 1;
    }

    public static Camera GetCurrentCamera()
    {
        if (isCameraMain)
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            return mainCamera.GetComponent<Camera>();
        }
        else
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("DynamicCamera");
            return mainCamera.GetComponent<Camera>();
        }
    }
}