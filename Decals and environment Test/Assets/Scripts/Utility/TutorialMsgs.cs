using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMsgs : MonoBehaviour
{

    public List<GameObject> TutorialList;
    public int tutorialNum;
    public GameObject tutorials;
    public GameObject activeTutorial;

    void Start()
    {
        tutorialNum = 1;



        foreach (Transform child in transform)
        {
            TutorialList.Add(child.gameObject);
            Debug.Log("The name of gameobject is " + child.name + " and the length of the list is " + TutorialList.Count);

            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ShowTutorialMessage(int tutorialNum)
    {
        activeTutorial = TutorialList[0];
    }
}
