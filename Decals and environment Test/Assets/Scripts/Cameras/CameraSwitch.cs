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

    AudioListener playerCamAud;
    AudioListener dynamicCamAud;


    public int cameraNum;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = theCharacter.GetComponent<Camera>();
        dynamicCamera = dynamicCameraGameObj.GetComponent<Camera>();
        controller = thefpsController.GetComponent<CharacterController>();
        playerCamAud = thefpsController.GetComponent<AudioListener>();
        dynamicCamAud = dynamicCameraGameObj.GetComponent<AudioListener>();

        cameraNum = 1;

        dynamicCamAud.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        cameraNum = BoardScript.camNum;


        if(Input.GetKey(KeyCode.J))
        {
            cameraNum = 1;
        }

        if (Input.GetKey(KeyCode.K))
        {
            cameraNum = 2;
        }

        if (cameraNum == 1)
        {
            thefpsController.SetActive(true);
            dynamicCameraHolder.SetActive(false);
            playerCamera.enabled = true;
            playerCamAud.enabled = true;
            controller.enabled = true;
            dynamicCamera.enabled = false;
            dynamicCamAud.enabled = false;
            dynamicCameraGameObj.transform.parent = dynamicCameraHolder.transform;
        }

        

        if (cameraNum == 2)
        {
            dynamicCameraHolder.SetActive(true);
            thefpsController.SetActive(false);
            dynamicCamera.enabled = true;
            dynamicCamAud.enabled = true;
            dynamicCameraGameObj.transform.parent = null;
            controller.enabled = false;
            playerCamera.enabled = false;
            playerCamAud.enabled = false;
        }

    }


   /* void CameraChange()
    {
        if (cameraNum == 1)
        {
            playerCamera.enabled = true;
            playerCamAud.enabled = true;
            dynamicCamera.enabled = false;
            dynamicCamAud.enabled = false;
        }

        if (cameraNum == 2)
        {
            
            dynamicCamera.enabled = true;
            dynamicCamAud.enabled = true;
            playerCamera.enabled = false;
            playerCamAud.enabled = false;
        }
    }*/
}
