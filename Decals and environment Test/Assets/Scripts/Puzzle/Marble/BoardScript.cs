using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : ObjectOfInterest, IInteractable
{
    public float limiter = 10f;
    public float rotspeed = 200;
    public float minSpeed = 1f;


    public bool marbleActve = false;
    public bool puzzleComplete = false;
    public List<GameObject> cameraViews;


    public static int camNum;


    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngZ;
    [SerializeField]
    float eulerAngXAlt;
    [SerializeField]
    float eulerAngZAlt;
    public GameObject localMarble;


    enum PuzzleState { ACTIVE = 0, PASSIVE = 2, SOLVED = 4 }
    PuzzleState state = PuzzleState.PASSIVE;

    Quaternion currentRot;
    public Quaternion startRot;
    Vector3 currentposition;

    public bool interacting = false;

    void Start()
    {
        startRot = transform.rotation;
        camNum = 1;

        foreach (Transform child in transform)
        {
            if (child.tag == "ViewPort")
            {
                Debug.Log("child found");
                cameraViews.Add(child.gameObject);
            }

        }
    }

    void Update()
    {
        marbleActve = MarbleBehaviour.marbleInInventory;
        puzzleComplete = MarbleFinish.puzzleComplete;
        
        //if (camNum == 2 && marbleActve == true && puzzleComplete == false)
        if(interacting)
        {
            localMarble.gameObject.SetActive(true);
        }


        if (interacting)
        {
            Invoke("RemoveCameras", .7f);
        }
        else
        {
            foreach (GameObject g in cameraViews)
            {
                g.transform.parent = this.transform;
            }
        }
        
        currentRot = transform.rotation;

        eulerAngX = transform.localEulerAngles.x;
        eulerAngY = transform.localEulerAngles.y;
        eulerAngZ = transform.localEulerAngles.z;

        eulerAngX = Mathf.Round(eulerAngX * 100f) / 100f;
        eulerAngY = Mathf.Round(eulerAngY * 100f) / 100f;
        eulerAngZ = Mathf.Round(eulerAngZ * 100f) / 100f;


        if (eulerAngX > 300) // 
        {
            eulerAngXAlt = eulerAngX - 360;
        }
        else
        {
            eulerAngXAlt = eulerAngX;
        }

        if (eulerAngZ > 300)
        {
            eulerAngZAlt = eulerAngZ - 360;
        }
        else
        {
            eulerAngZAlt = eulerAngZ;
        }


        if (camNum == 2 && Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Return))
        {
            camNum = 1;
        }


        if (interacting == true)
        {
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
            {
                if (eulerAngXAlt > -10)
                {
                    transform.Rotate(Vector3.left * rotspeed * Time.deltaTime * 20);
                }
            }
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
            {
                if (eulerAngX < 10 || eulerAngX > 340)
                {
                    transform.Rotate(Vector3.right * rotspeed * Time.deltaTime * 20);
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (eulerAngZ < 10 || eulerAngZ > 340)
                {
                    transform.Rotate(Vector3.forward * rotspeed * Time.deltaTime * 20);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (eulerAngZAlt > -10)
                {
                    transform.Rotate(Vector3.back * rotspeed * Time.deltaTime * 20);
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.up * rotspeed * Time.deltaTime * 20);
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.down * rotspeed * Time.deltaTime * 20);
            }

        }




        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Working");
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, rotspeed * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            return;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, rotspeed * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.Space))
        {
            interacting = false;
        }
    }
    

    public override void FocusCamera()
    {
        base.FocusCamera();
        interacting = true;
        Debug.Log("The Changed class was called");
    }


    public void RemoveCameras()
    {
        foreach (GameObject g in cameraViews)
        {
            g.transform.parent = null;
        }
    }

    void IInteractable.InteractWith()
    {
        if (state == (PuzzleState.ACTIVE | PuzzleState.SOLVED)) { return; }

        state = PuzzleState.ACTIVE;

        GameStateManager.SetGameState(GameState.INTERACTING_W_ITEM); //TECHNICALLY this should be the part where the camera puts the bookshelf in focus

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}