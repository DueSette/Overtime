using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerScript : MonoBehaviour, IInteractable
{
    [SerializeField] List<EmailScriptableObject> containedEmails = new List<EmailScriptableObject>();

    private readonly List<GameObject> EmailUIGameObjects = new List<GameObject>(); //internal reference to the present mails

    [SerializeField] GameObject emailEntryPrefab;
    [SerializeField] GameObject leftSidePanel;

    [SerializeField] TextMeshProUGUI rightPanelTitle, rightPanelPeople, rightPanelBody;
    [SerializeField] Image rightPanelEmailVeil;
    [SerializeField] Image spaceTooltip;
    [SerializeField] GameObject standbyScreen;
    [SerializeField] AudioClip clickSound, logon, logoff;
    bool beingInteractedWith = false;

    public delegate void ReadEmailDelegate(string s);
    public static event ReadEmailDelegate ReadEmailEvent;

    #region Unity methods
    private void OnEnable()
    {
        //possible setup logic
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += LeaveInteraction;
        EmailClickUtility.EmailDeselectEvent += CleanRightPanel;
        Initialise();
    }
    private void OnDisable()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= LeaveInteraction;
        EmailClickUtility.EmailDeselectEvent -= CleanRightPanel;
    }

    private void Update() //quick and dirty scrolling sound logic
    {
        if(beingInteractedWith == false) { return; }
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SoundManager.instance.PlaySound(clickSound);
    }
    #endregion

    #region Email operations related methods
    void Initialise()
    {
        standbyScreen.SetActive(true);

       //load emails into worldspace canvas
       foreach(EmailScriptableObject email in containedEmails)
        {
            GameObject g = Instantiate(emailEntryPrefab, leftSidePanel.transform);
            g.GetComponentInChildren<TextMeshProUGUI>().SetText(email.title);
            g.SetActive(true);
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

    public void DisplayEmailOnClick(TextMeshProUGUI title) //called via the button component on the object
    {
        foreach(EmailScriptableObject email in containedEmails)
            if (email.title == title.text)
                DisplayEmail(email);
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

    #region Interaction related methods
    void IInteractable.InteractWith() //removes standby screen, wakes up cursor
    {
        beingInteractedWith = true;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        spaceTooltip.enabled = true;
        SoundManager.instance.PlaySound(logon);
        standbyScreen.SetActive(false);
       // CheckIfPlayerReadEmails();

        DisplayEmail(containedEmails[0]);
        EmailUIGameObjects[0].GetComponent<Button>().Select();         
    }

    public void LeaveInteraction() //reverts the computer back to how it was before being interacted with
    {
        if(!beingInteractedWith) { return; }
        beingInteractedWith = false;

        spaceTooltip.enabled = false;
        SoundManager.instance.PlaySound(logoff);
        standbyScreen.SetActive(true);
    }

    #endregion
}
