using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rattest : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            Debug.Log("working");
            animator.SetBool("Sniff", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Idle", true);
        }

        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("working");
            animator.SetBool("Idle", false);
            animator.SetBool("Sniff", false);
            animator.SetBool("Walking", true);
        }

        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("working");
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Sniff", true);
        }
    }
}
