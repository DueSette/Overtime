using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerCameraSystem : ComputerSystem
{
    [SerializeField] List<string> camerasToConnect = new List<string>();
    private List<ViewableCamera> connectedCameras = new List<ViewableCamera>();

    private readonly List<GameObject> CameraUIGameObjects = new List<GameObject>(); //internal reference to the present cameras

    [SerializeField] GameObject cameraEntryPrefab;
    [SerializeField] GameObject leftSidePanel;

    [SerializeField] TextMeshProUGUI rightPanelCameraName;
    [SerializeField] GameObject rightPanelRecordingIcon;
    [SerializeField] RawImage rightPanelCameraView;
    [SerializeField] Image rightPanelCameraVeil;

    [SerializeField] AudioClip clickSound;

    public delegate void ViewCameraDelegate(string s);
    public static event ViewCameraDelegate ViewCameraEvent;

    #region Unity methods
    private void OnEnable()
    {
        //possible setup logic
        EmailClickUtility.EmailDeselectEvent += CleanRightPanel;
        Initialise();
    }
    private void OnDisable()
    {
        EmailClickUtility.EmailDeselectEvent -= CleanRightPanel;
    }
    #endregion


    #region computer system related methods
    public override void StartupSystem()
    {
        DisplayCamera(connectedCameras[0]);
        connectedCameras[0].SetCameraEnabled(true);
        CameraUIGameObjects[0].GetComponent<Button>().Select();
    }

    public override void UpdateSystem()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.instance.PlaySound(clickSound);
        }
    }

    public override void ShutdownSystem()
    {
        foreach (ViewableCamera camera in connectedCameras)
                camera.SetCameraEnabled(false);
    }
    #endregion

    #region Camera operations related methods
    void Initialise()
    {
        //load emails into worldspace canvas
        ViewableCamera[] viewableCameras = GameObject.FindObjectsOfType<ViewableCamera>();
        foreach (string code in camerasToConnect)
        {
            // Finding Camera in scene
            ViewableCamera newConnection = new ViewableCamera();
            foreach (ViewableCamera vc in viewableCameras)
            {
                if (vc.cameraCode == code)
                {
                    newConnection = vc;
                }
            }

            if (newConnection != null)
            {
                connectedCameras.Add(newConnection);
            }
            else
            {
                Debug.LogError("Error: Requested Viewable Camera Does Not Exist In Scene (Camera Code - " + code + ")");
            }


            GameObject g = Instantiate(cameraEntryPrefab, leftSidePanel.transform);
            g.GetComponentInChildren<TextMeshProUGUI>().SetText(newConnection.cameraName);
            g.SetActive(true);
            CameraUIGameObjects.Add(g);
        }
    }

    private void DisplayCamera(ViewableCamera camera) //fills the right side display with the content of the selected email scriptable object's data
    {
        rightPanelCameraName.SetText("Cam: " + camera.cameraCode);
        rightPanelRecordingIcon.SetActive(true);

        rightPanelCameraView.texture = camera.cameraView;
        rightPanelCameraView.color = Color.white;

        rightPanelCameraVeil.enabled = true;

            
    }

    public void CleanRightPanel()
    {
        rightPanelCameraName.SetText("Cam: ----");
        rightPanelRecordingIcon.SetActive(false);

        rightPanelCameraView.texture = null;
        rightPanelCameraView.color = Color.black;

        rightPanelCameraVeil.enabled = false;
    }

    public void DisplayEmailOnClick(TextMeshProUGUI title) //called via the button component on the object
    {
        foreach (ViewableCamera camera in connectedCameras)
            if (camera.cameraName == title.text)
            {
                camera.SetCameraEnabled(true);
                DisplayCamera(camera);
            }
        else
            {
                camera.SetCameraEnabled(false);
            }
    }

    /*
    private void CheckIfPlayerReadEmails() // [NOT NEEDED FOR NOW] checks if email title was added to a list of emails that were read once already and acts upon it
    {
        foreach (GameObject mail in EmailUIGameObjects)
        {
            string mailTitle = mail.GetComponentInChildren<TextMeshProUGUI>().text;

            if (!EmailManager.HasReadEmail(mailTitle)) //if the email in the PC was never read it will have a different font appearance
            {
                mail.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                mail.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            }
            else
            {
                mail.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                mail.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            }
        }
    }
    */
    #endregion
}
