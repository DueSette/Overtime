using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraScript : MonoBehaviour
{
    public float eulerAngY;

    public int camPos;
    public bool camMove;
    public float camSpeed;

    private int maxDistance;
    private int minDistance;


    private void Start()
    {
        camPos = 1;
        camMove = true;
        camSpeed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {

        eulerAngY = transform.localEulerAngles.y;
        eulerAngY = Mathf.Round(eulerAngY * 100f) / 100f;


        if (eulerAngY > 230 && eulerAngY < 240 && camPos == 2)
        {
            camPos = 1;
            StartCoroutine(RotateCamera());
        }


        if (eulerAngY > 300 && eulerAngY < 310 && camPos == 1)
        {
            camPos = 2;
            StartCoroutine(RotateCamera());
        }

        if (camMove == true && camPos == 1)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * camSpeed);
        }

        if (camMove == true && camPos == 2)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -camSpeed);
        }
    }


    IEnumerator RotateCamera()
    {
        camMove = false;
        yield return new WaitForSeconds(2.0f);
        camMove = true;
    }


    private void OnDrawGizmosSelected()
    {
        // Drawing Cone Of Movement
        Vector3 camPos = this.transform.position;
        Vector3 camStartLook = this.transform.position + this.transform.forward;
        Vector3 camEndLook = this.transform.position + (Quaternion.AngleAxis(90, Vector3.up) * this.transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(camPos, camStartLook);
        Gizmos.DrawLine(camPos, camEndLook);
        Gizmos.DrawLine(camStartLook, camEndLook);
    }
}
