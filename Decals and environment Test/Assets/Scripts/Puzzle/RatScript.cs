using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScript : MonoBehaviour
{
    public GameObject theRat;
    public GameObject ratAnimObj;
    Rigidbody r_RigidBody;
    public float ratSpeed;
    public float ratDistance;
    public int ratState;
    public GameObject ratSensor;
    bool hasCollided;


    float turnTime;
    public int directionNum;

    RaycastHit hit;
    Ray ratRay;

    Vector3 testRay;


    private Animation speakerAnim;
    public Animator animator;


    //Variables that control Rat Rotation
    public List<Transform> directionList;
    public List<GameObject> speakers;
    public Transform currentRotation;
    public GameObject currentSpeaker;
    public float transitionSpeed;
    public int speakerNumber;

    // Start is called before the first frame update
    void Start()
    {
        ratState = 2;
        directionNum = 0;
        turnTime = 1.8f;
        animator = ratAnimObj.transform.GetComponent<Animator>();
        currentSpeaker = speakers[directionNum];
        currentRotation = directionList[directionNum];
        speakerAnim = currentSpeaker.GetComponent<Animation>();
        theRat = this.gameObject;
        r_RigidBody = theRat.GetComponent<Rigidbody>();
        ratDistance = 1f;
        ratSpeed = 1f;
        transitionSpeed = 3F;
        hasCollided = false;
    }



    // Update is called once per frame
    void Update()
    {
        currentSpeaker = speakers[directionNum];
        speakerAnim = currentSpeaker.GetComponent<Animation>();
        currentRotation = directionList[directionNum];


        Vector3 forward = theRat.transform.TransformDirection(Vector3.forward) * ratDistance;
        Vector3 backward = theRat.transform.TransformDirection(Vector3.back) * ratDistance;
        Vector3 left = theRat.transform.TransformDirection(Vector3.left) * ratDistance;
        Vector3 right = theRat.transform.TransformDirection(Vector3.right) * ratDistance;

        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            speakerAnim.Play("SpeakerAnim");
        }



        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           // ratRay = new Ray(theRat.transform.position, Vector3.forward);
            testRay = forward;
            directionNum = 0;
            ratState = 3;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          //  ratRay = new Ray(theRat.transform.position, Vector3.right);
            testRay = right;
            directionNum = 1;
            ratState = 3;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          //  ratRay = new Ray(theRat.transform.position, Vector3.back);
            testRay = backward;
            directionNum = 2;
            ratState = 3;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           // ratRay = new Ray(theRat.transform.position, Vector3.left);
            testRay = left;
            directionNum = 3;
            ratState = 3;
        }

        switch (ratState)
        {
            case 1:
                animator.SetBool("Idle", false);
                animator.SetBool("Sniff", false);
                animator.SetBool("Walking", true);
                    if (directionNum == 0)
                {
                    ratRay = new Ray(theRat.transform.position, Vector3.forward);
                    r_RigidBody.position += Vector3.forward * Time.deltaTime * ratSpeed;
                }
                else
                    if (directionNum == 1)
                {
                    ratRay = new Ray(theRat.transform.position, Vector3.right);
                    r_RigidBody.position += Vector3.right * Time.deltaTime * ratSpeed;
                }
                else
                    if (directionNum == 2)
                {
                    ratRay = new Ray(theRat.transform.position, Vector3.back);
                    r_RigidBody.position += Vector3.back * Time.deltaTime * ratSpeed;
                }
                else
                    if (directionNum == 3)
                {
                    ratRay = new Ray(theRat.transform.position, Vector3.left);
                    r_RigidBody.position += Vector3.left * Time.deltaTime * ratSpeed;
                }
                    break;

            case 2:
                animator.SetBool("Sniff", false);
                animator.SetBool("Walking", false);
                animator.SetBool("Idle", true);
                break;

            case 3:
                turnTime -= Time.deltaTime;
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);
                animator.SetBool("Sniff", true);

                if(turnTime < 0)
                {
                    ratState = 1;
                    hasCollided = false;
                }
                break;

            default:
                Debug.Log("Ratstate is undeclared");
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(ratRay, out hit, ratDistance))
        {
            if (hasCollided == false)
            {
                if (hit.collider.tag == "Block")
                {
                    Debug.Log("collider is " + hit.collider.name);
                    hasCollided = true;
                    ratState = 2;
                    return;
                }
            }
        }
    }



    private void LateUpdate()
    {

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentRotation.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentRotation.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentRotation.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }

}
