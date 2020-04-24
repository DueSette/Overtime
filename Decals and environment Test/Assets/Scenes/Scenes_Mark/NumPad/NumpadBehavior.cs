using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumpadBehavior : ObjectOfInterest
{

    /// <summary>
    /// Code used to control all functions of the keypad, and to unlock doors.
    /// 
    /// 
    /// Only variables to be assigned in inspector is the text boxes for the acces code, and current code used.
    /// </summary>

    public delegate void PasswordDelegate(string s);
    public static event PasswordDelegate PasswordEvent;


    AudioSource audioSource;

    public GameObject numpad1;
    public GameObject numpad2;
    public GameObject numpad3;
    public GameObject numpad4;
    public GameObject numpad5;
    public GameObject numpad6;
    public GameObject numpad7;
    public GameObject numpad8;
    public GameObject numpad9;
    public GameObject numpad0;
    public GameObject accessBar;

    public string state;
    bool open = false;

    [SerializeField] private AudioClip BtnSound;
    [SerializeField] private AudioClip GrantedSound;
    [SerializeField] private AudioClip DeniedSound;

    public string currentCode;
    public int accessCode;
    public string accessCodeString;

    public EmailScriptableObject codeEmail;

    private void OnEnable()
    {
        LevelManager.onLevelEvent += InitNumpadEvent;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= InitNumpadEvent;

        // Reverting Email
        string s = codeEmail.body;
        string c = accessCode.ToString();
        s = s.Replace(c, "XXXX");
        codeEmail.body = s;
    }

    private void InitNumpadEvent(string eventCode)
    {
        if (eventCode == "LevelStart")
        {
            accessCode = Random.Range(1000, 9999);
            Debug.Log("access code is " + accessCode);

            // Showing Code In Email
            string s = codeEmail.body;
            string c = accessCode.ToString();
            s = s.Replace("XXXX", c);
            codeEmail.body = s;

            accessCodeString = accessCode.ToString();
            Debug.Log("access code(ToString) is " + accessCodeString);
            audioSource = GetComponent<AudioSource>();
            generateCode();
            state = "NotInteracting";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            state = "NotInteracting";
        }

        switch (state)
        {
            case "NotInteracting":
                {
                    currentCode = "";
                }
                break;

            case "Unanswered":
                if (currentCode.Length < 4)
                {

                    if (Input.GetKeyDown("1") || Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        StartCoroutine(ButtonPressAnim(numpad1));
                        //numpad1.GetComponent<Animation>().Play("Numpad1Anim");
                        currentCode = currentCode + "1";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("2") || Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        StartCoroutine(ButtonPressAnim(numpad2));
                        //numpad2.GetComponent<Animation>().Play("Numpad2Anim");
                        currentCode = currentCode + "2";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("3") || Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        StartCoroutine(ButtonPressAnim(numpad3));
                        //numpad3.GetComponent<Animation>().Play("Numpad3Anim");
                        currentCode = currentCode + "3";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("4") || Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        StartCoroutine(ButtonPressAnim(numpad4));
                        //numpad4.GetComponent<Animation>().Play("Numpad4Anim");
                        currentCode = currentCode + "4";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("5") || Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        StartCoroutine(ButtonPressAnim(numpad5));
                        //numpad5.GetComponent<Animation>().Play("Numpad5Anim");
                        currentCode = currentCode + "5";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("6") || Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        StartCoroutine(ButtonPressAnim(numpad6));
                        //numpad6.GetComponent<Animation>().Play("Numpad6Anim");
                        currentCode = currentCode + "6";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("7") || Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        StartCoroutine(ButtonPressAnim(numpad7));
                        //numpad7.GetComponent<Animation>().Play("Numpad7Anim");
                        currentCode = currentCode + "7";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("8") || Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        StartCoroutine(ButtonPressAnim(numpad8));
                        //numpad8.GetComponent<Animation>().Play("Numpad8Anim");
                        currentCode = currentCode + "8";
                        ButtonPress();
                    }

                    if (Input.GetKeyDown("9") || Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        StartCoroutine(ButtonPressAnim(numpad9));
                        //numpad9.GetComponent<Animation>().Play("Numpad9Anim");
                        currentCode = currentCode + "9";
                        ButtonPress();
                    }
                    if (Input.GetKeyDown("0") || Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        StartCoroutine(ButtonPressAnim(numpad0));
                        //numpad0.GetComponent<Animation>().Play("Numpad0Anim");
                        currentCode = currentCode + "0";
                        ButtonPress();
                    }

                }
                else
                    if (currentCode == accessCodeString)
                {
                    accessBar.GetComponent<Animation>().Play("AccessGrantedAnim");
                    audioSource.PlayOneShot(GrantedSound);
                    OpenableDoor.OnDoorUnlockEvent("ExecOfficeUnlock");
                    state = "Complete";
                    GameStateManager.SetGameState(GameState.IN_GAME);
                }
                else
                    if (currentCode != accessCodeString)
                {
                    currentCode = "";
                    audioSource.PlayOneShot(DeniedSound);
                    accessBar.GetComponent<Animation>().Play("AccessDeniedAnim");
                    Debug.Log("state is " + state);
                }
                break;

            case "Complete":
                {
                    currentCode = "";
                }
                break;
            case "Inactive":
                {
                }
                break;
        }
    }

    private void ButtonPress()
    {
        audioSource.PlayOneShot(BtnSound);
    }

    public override void FocusCamera()
    {
        base.FocusCamera();
        state = "Unanswered";
        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);
        Debug.Log("The Changed class was called");
    }

    void generateCode()
    {
        string password = accessCodeString;
        Debug.Log("Password is " + accessCodeString);
        //PasswordEvent(accessCodeString);
    }

    private IEnumerator ButtonPressAnim(GameObject pressedButton)
    {
        Vector3 startTransform = pressedButton.transform.localPosition;

        float t = 0;
        while (t < 1.0f)
        {
            t += (Time.deltaTime / 0.4f);

            Vector3 currentTransform = startTransform;
            currentTransform.x = (Mathf.Sin(t * 3.14f)) * 0.002f;

            pressedButton.transform.localPosition = currentTransform;

            yield return null;

        }

        pressedButton.transform.localPosition = startTransform;
    }
}
