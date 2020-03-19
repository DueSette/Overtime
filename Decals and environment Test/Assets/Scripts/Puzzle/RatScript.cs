using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScript : MonoBehaviour
{
    public GameObject theRat;
    Rigidbody r_RigidBody;
    public float ratSpeed;
    public float ratDistance;
    public int ratState;

    public int directionNum;


    //Variables that control Rat Rotation
    public List<Transform> directionList;
    public Transform currentRotation;
    public float transitionSpeed;

    private Quaternion lookRotation;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        ratState = 2;
        directionNum = 0;
        currentRotation = directionList[directionNum];
        theRat = this.gameObject;
        r_RigidBody = theRat.GetComponent<Rigidbody>();
        ratDistance = 1f;
        ratSpeed = 1f;
        transitionSpeed = 3F;
    }

    // Update is called once per frame
    void Update()
    {

        currentRotation = directionList[directionNum];


        Vector3 forward = theRat.transform.TransformDirection(Vector3.forward) * ratDistance;

        RaycastHit hit;
        Ray ratRay = new Ray(theRat.transform.position, Vector3.forward);
        Debug.DrawRay(theRat.transform.position, forward, Color.red);


        if(Input.GetKeyDown(KeyCode.A))
        {
            ratState++;
            if (ratState > 3)
                ratState = 1;
        }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("viewnum is " + directionNum);
                directionNum = directionNum - 1;

                if (directionNum < 0)
                {
                directionNum = directionList.Count - 1;
                }

                currentRotation = directionList[directionNum];
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("viewnum is " + directionNum);

            directionNum = directionNum + 1;

                if (directionNum > directionList.Count)
                {
                directionNum = 1;
                }
                Debug.Log(directionNum);
            currentRotation = directionList[directionNum];
            }


            switch (ratState)
        {
            case 1:

                r_RigidBody.position += Vector3.forward * Time.deltaTime * ratSpeed;
                break;

            case 2:

                break;

            case 3:
                Debug.Log("Rat is turning");
                break;

            default:
                Debug.Log("Ratstate is undeclared");
                break;
        }
 


        if(Physics.Raycast(ratRay,out hit, ratDistance))
        {
            if(hit.collider)
            {
                ratState = 2;
            }
        }

    }


    void stopMovement()
    {
        ratSpeed = 0;
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
