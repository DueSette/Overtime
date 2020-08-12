using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumpadBehavior : MonoBehaviour, IInteractable
{
    public delegate void PasswordDelegate(string s);
    public static event PasswordDelegate PasswordEvent;

    [Header("Numpad Components")]
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
    public Collider numpadCollider;

    enum PuzzleState { ACTIVE = 0, PASSIVE = 2, SOLVED = 4 }
    PuzzleState state = PuzzleState.PASSIVE;

    [Header("Audio")]
    [SerializeField] private AudioClip BtnSound;
    [SerializeField] private AudioClip GrantedSound;
    [SerializeField] private AudioClip DeniedSound;
    AudioSource audioSource;

    [Header("Solution Code")]
    public string currentCode;
    public int accessCode;
    public string accessCodeString;
    public Text accessCodeUI;
    public EmailScriptableObject codeEmail;


    private void OnEnable()
    {
        LevelManager.onLevelEvent += InitNumpadEvent;
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction += ExitInteraction;
    }

    private void OnDisable()
    {
        LevelManager.onLevelEvent -= InitNumpadEvent;
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.ExitInteraction -= ExitInteraction;

        // Reverting Email
        string s = codeEmail.body;
        string c = accessCode.ToString();
        s = s.Replace(c, "XXXX");
        codeEmail.body = s;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (state != PuzzleState.ACTIVE) { return; }


        if (Input.GetKey(KeyCode.Space))
        {
            state = PuzzleState.PASSIVE;
        }


        if (currentCode.Length < 4)
        {
            CheckMouseInput();
            CheckKeyboardInput();
        }
        else
        {
            // Checking if puzzle has been solved
            if (currentCode == accessCodeString)
            {
                accessBar.GetComponent<Animation>().Play("AccessGrantedAnim");
                audioSource.PlayOneShot(GrantedSound);
                OpenableDoor.OnDoorUnlockEvent("ExecOfficeUnlock");

                state = PuzzleState.SOLVED;
                GameStateManager.SetGameState(GameState.IN_GAME);


                Destroy(GetComponent<ObjectOfInterest>());
                Destroy(this);
            }
            else
            {
                currentCode = "";
                Invoke("UpdateUI", 1.4f);

                audioSource.PlayOneShot(DeniedSound);
                accessBar.GetComponent<Animation>().Play("AccessDeniedAnim");
                Debug.Log("state is " + state);
            }
        }
    }


    /*
    ====================================================================================================
    Interaction
    ====================================================================================================     
    */
    void IInteractable.InteractWith()
    {
        if (state == (PuzzleState.ACTIVE | PuzzleState.SOLVED))
        {
            return;
        }

        state = PuzzleState.ACTIVE;
        currentCode = "";
        //numpadCollider.enabled = false;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera dynamicCamera = GameObject.FindGameObjectWithTag("DynamicCamera").GetComponent<Camera>();

            Ray ray = dynamicCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] allHit = Physics.RaycastAll(ray, 2);
            foreach (RaycastHit hit in allHit)
            {
                if (hit.collider.gameObject.tag == "Keypad")
                {
                    string s = hit.collider.gameObject.name;
                    int number = int.Parse(s.Substring(s.Length - 1));

                    ButtonPress(number);

                    break;
                }
            }
        }
    }

    void CheckKeyboardInput()
    {
        if (Input.GetKeyDown("1"))
        {
            ButtonPress(1);
        }

        if (Input.GetKeyDown("2"))
        {
            ButtonPress(2);
        }

        if (Input.GetKeyDown("3"))
        {
            ButtonPress(3);
        }

        if (Input.GetKeyDown("4"))
        {
            ButtonPress(4);
        }

        if (Input.GetKeyDown("5"))
        {
            ButtonPress(5);
        }

        if (Input.GetKeyDown("6"))
        {
            ButtonPress(6);
        }

        if (Input.GetKeyDown("7"))
        {
            ButtonPress(7);
        }

        if (Input.GetKeyDown("8"))
        {
            ButtonPress(8);
        }

        if (Input.GetKeyDown("9"))
        {
            ButtonPress(9);
        }
        if (Input.GetKeyDown("0"))
        {
            ButtonPress(0);
        }
    }

    void ExitInteraction()
    {
        if (state == PuzzleState.PASSIVE) { return; }

        state = PuzzleState.PASSIVE;
        //numpadCollider.enabled = true;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    /*
    ====================================================================================================
    Solution Generation
    ====================================================================================================     
    */
    void generateCode()
    {
        string password = accessCodeString;
        Debug.Log("Password is " + accessCodeString);
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
            state = PuzzleState.PASSIVE;
        }
    }


    /*
    ====================================================================================================
    Animation
    ====================================================================================================     
    */
    private void ButtonPress(int buttonNumber)
    {
        if (audioSource == null)
        {
            audioSource = this.GetComponent<AudioSource>();
        }
        audioSource.PlayOneShot(BtnSound);

        switch (buttonNumber)
        {
            case (1):
                {
                    StartCoroutine(ButtonPressAnim(numpad1));
                    currentCode = currentCode + "1";
                }
                break;

            case (2):
                {
                    StartCoroutine(ButtonPressAnim(numpad2));
                    currentCode = currentCode + "2";
                }
                break;

            case (3):
                {

                    StartCoroutine(ButtonPressAnim(numpad3));
                    currentCode = currentCode + "3";
                }
                break;

            case (4):
                {
                    StartCoroutine(ButtonPressAnim(numpad4));
                    currentCode = currentCode + "4";
                }
                break;

            case (5):
                {
                    StartCoroutine(ButtonPressAnim(numpad5));
                    currentCode = currentCode + "5";
                }
                break;

            case (6):
                {
                    StartCoroutine(ButtonPressAnim(numpad6));
                    currentCode = currentCode + "6";
                }
                break;

            case (7):
                {

                    StartCoroutine(ButtonPressAnim(numpad7));
                    currentCode = currentCode + "7";
                }
                break;

            case (8):
                {
                    StartCoroutine(ButtonPressAnim(numpad8));
                    currentCode = currentCode + "8";
                }
                break;

            case (9):
                {
                    StartCoroutine(ButtonPressAnim(numpad9));
                    currentCode = currentCode + "9";
                }
                break;

            case (0):
                {
                    StartCoroutine(ButtonPressAnim(numpad0));
                    currentCode = currentCode + "0";
                }
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        accessCodeUI.text = currentCode;
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
