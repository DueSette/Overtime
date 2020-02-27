using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("working");
            this.transform.position = this.transform.position + new Vector3(transform.position.x, transform.position.y + 1,
                                                                        transform.position.z);
        }


        this.transform.position = this.transform.position + new Vector3(transform.position.x, transform.position.y + 1,
                                                                        transform.position.z);
    }
}
