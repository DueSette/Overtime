using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{

    public Transform[] views;
    public GameObject fpsController;
    public List<Transform> viewList; // Make stuff assigned to list tomorrow
    public float transitionSpeed;
    Transform currentView;
    public Transform startView;
    public GameObject cameraHolder;
    public int viewNum;
    public int maxviews;
    public Camera dynamicCamera;
    public bool tester;


    // Start is called before the first frame update
    void Start()
    {
        currentView = views[0];
        cameraHolder = GameObject.FindGameObjectWithTag("DynamicCameraHolder");
        startView = cameraHolder.transform;
        dynamicCamera = this.gameObject.GetComponent<Camera>();
        viewList.Add(startView);
        viewNum = 0;
        currentView = viewList[viewNum];
        transitionSpeed = 3F;
        maxviews = views.Length;
    }


    void Update()
    {
        currentView = viewList[viewNum];
        //---------------------------------------
        // Functions for cycling between cameras,
        //---------------------------------------


        if (dynamicCamera.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("viewnum is " + viewNum);
                viewNum = viewNum - 1;

                if (viewNum < 1)
                {
                    viewNum = viewList.Count - 1;

                }

                currentView = viewList[viewNum];
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("viewnum is " + viewNum);

                viewNum = viewNum + 1;

                if (viewNum > viewList.Count - 1)
                {
                    viewNum = 1;
                }
                Debug.Log(viewNum);
                currentView = viewList[viewNum];
            }
        }
    }



    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }

    IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(1f);
    }
}



