using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{

    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    public int viewNum;
    public int maxviews;
    public Camera marbleCamera;


    // Start is called before the first frame update
    void Start()
    {
        currentView = views[4];
        marbleCamera = this.gameObject.GetComponent<Camera>();
        viewNum = 5;
        transitionSpeed = .5F;
        maxviews = views.Length;
    }


    void Update()
    {
        if (marbleCamera.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("hitting R");
                viewNum = viewNum - 1;

                if (viewNum < 0)
                {
                    viewNum = views.Length - 1;

                }

                currentView = views[viewNum];
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("hitting R");

                viewNum = viewNum + 1;

                if (viewNum > views.Length - 1)
                {
                    viewNum = 0;
                }

                Debug.Log(viewNum);
                currentView = views[viewNum];
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }

}

  

