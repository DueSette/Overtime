using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumpadBehavior : MonoBehaviour
{

    /// <summary>
    /// Code used to control all functions of the keypad, and to unlock doors.
    /// 
    /// 
    /// Only variables to be assigned in inspector is the text boxes for the acces code, and current code used.
    /// </summary>

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
    public GameObject currentCodeTextBox;
    public GameObject accessCodeTextBox;
    public string state;
    public GameObject theDoor;
    public Animator theDoorAnimator;
    bool open = false;
    private OpenableDoor DoorScript;


    public string currentCode;
    public int accessCode;
    public string accessCodeString;

    public void Start()
    {
        DoorScript = theDoor.GetComponent<OpenableDoor>();
        DoorScript.canBeOpened = false;
        //accessCode = Random.Range(1000, 9999);
        accessCode = 2972;
        Debug.Log("access code is " + accessCode);
        accessCodeString = accessCode.ToString();
        Debug.Log("access code(ToString) is " + accessCodeString);

        state = "Unanswered";
    }


    // Update is called once per frame
    void Update()
    {

        accessCodeTextBox.GetComponent<Text>().text = "" + accessCode;

        switch (state)
        {
            case "Unanswered":
                if (currentCode.Length < 4)
                {

                    if (Input.GetKeyDown("1"))
                    {
                        numpad1.GetComponent<Animation>().Play("Numpad1Anim");
                        currentCode = currentCode + "1";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("2"))
                    {
                        numpad2.GetComponent<Animation>().Play("Numpad2Anim");
                        currentCode = currentCode + "2";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("3"))
                    {
                        numpad3.GetComponent<Animation>().Play("Numpad3Anim");
                        currentCode = currentCode + "3";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("4"))
                    {
                        numpad4.GetComponent<Animation>().Play("Numpad4Anim");
                        currentCode = currentCode + "4";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("5"))
                    {
                        numpad5.GetComponent<Animation>().Play("Numpad5Anim");
                        currentCode = currentCode + "5";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("6"))
                    {
                        numpad6.GetComponent<Animation>().Play("Numpad6Anim");
                        currentCode = currentCode + "6";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("7"))
                    {
                        numpad7.GetComponent<Animation>().Play("Numpad7Anim");
                        currentCode = currentCode + "7";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("8"))
                    {
                        numpad8.GetComponent<Animation>().Play("Numpad8Anim");
                        currentCode = currentCode + "8";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                    if (Input.GetKeyDown("9"))
                    {
                        numpad9.GetComponent<Animation>().Play("Numpad9Anim");
                        currentCode = currentCode + "9";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }
                    if (Input.GetKeyDown("0"))
                    {
                        numpad0.GetComponent<Animation>().Play("Numpad0Anim");
                        currentCode = currentCode + "0";
                        currentCodeTextBox.GetComponent<Text>().text = currentCode;
                    }

                }
                else
                    if (currentCode == accessCodeString)
                {
                    accessBar.GetComponent<Animation>().Play("AccessGrantedAnim");
                    DoorScript.canBeOpened = true;
                    state = "Complete";
                }
                else
                    if (currentCode != accessCodeString)
                {
                    currentCode = "";
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

    void UnlockDoor()
    {
        Debug.Log("IT SHOULD BE ANIMATING");
                    theDoor.GetComponent<Animation>().Play("DoorOpen");
    }
}
