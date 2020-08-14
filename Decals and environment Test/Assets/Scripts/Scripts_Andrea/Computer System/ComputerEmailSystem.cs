using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerEmailSystem : ComputerSystem
{
    [SerializeField] List<EmailScriptableObject> containedEmails = new List<EmailScriptableObject>();

    private readonly List<GameObject> EmailUIGameObjects = new List<GameObject>(); //internal reference to the present mails

    [SerializeField] GameObject emailEntryPrefab;
    [SerializeField] GameObject leftSidePanel;

    [SerializeField] TextMeshProUGUI rightPanelTitle, rightPanelPeople, rightPanelBody;
    [SerializeField] Image rightPanelEmailVeil;

    [SerializeField] AudioClip clickSound;

    public delegate void ReadEmailDelegate(string s);
    public static event ReadEmailDelegate ReadEmailEvent;

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
        DisplayEmail(containedEmails[0]);
        EmailUIGameObjects[0].GetComponent<Button>().Select();
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
        base.ShutdownSystem();
    }
    #endregion

    #region Email operations related methods
    void Initialise()
    {
        //load emails into worldspace canvas
        foreach (EmailScriptableObject email in containedEmails)
        {
            GameObject g = Instantiate(emailEntryPrefab, leftSidePanel.transform);
            g.GetComponentInChildren<TextMeshProUGUI>().SetText(email.title);
            g.SetActive(true);

            g.GetComponent<EmailClickUtility>().data = email.title;

            EmailUIGameObjects.Add(g);
        }
    }

    private void DisplayEmail(EmailScriptableObject email) //fills the right side display with the content of the selected email scriptable object's data
    {
        rightPanelTitle.SetText(email.title);
        rightPanelPeople.SetText("From: " + email.sender + ". To: " + email.receivers);
        rightPanelBody.SetText(email.body);

        rightPanelEmailVeil.enabled = true;
        ReadEmailEvent?.Invoke(email.title);
    }

    public void CleanRightPanel()
    {
        rightPanelTitle.SetText("");
        rightPanelPeople.SetText("");
        rightPanelBody.SetText("");
        rightPanelEmailVeil.enabled = false;
    }

    public override void DisplayOnClick(string data) //called via the button component on the object
    {
        foreach (EmailScriptableObject email in containedEmails)
        {
            if (email.title == data)
            {
                DisplayEmail(email);
                break;
            }
        }
    }

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
    #endregion
}
