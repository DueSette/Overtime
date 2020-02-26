using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleFinish : MonoBehaviour
{
    public static bool puzzleComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        puzzleComplete = true;
        Destroy(other.gameObject);
    }
}
