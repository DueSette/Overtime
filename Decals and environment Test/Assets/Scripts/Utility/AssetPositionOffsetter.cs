using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetPositionOffsetter : MonoBehaviour
{
    [SerializeField] private float maxOffset;

    [SerializeField] private bool offsetOnXAxis;
    [SerializeField] private bool offsetOnYAxis;
    [SerializeField] private bool offsetOnZAxis;

    public void OffsetPositions()
    {
#if UNITY_EDITOR
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject g in selectedObjects)
        {
            Vector3 startPos = g.transform.position;

            // X Axis
            if (offsetOnXAxis)
            {
                startPos.x += (Random.Range(-maxOffset, maxOffset));
            }

            // Y Axis
            if (offsetOnYAxis)
            {
                startPos.y += (Random.Range(-maxOffset, maxOffset));
            }

            // Z Axis
            if (offsetOnZAxis)
            {
                startPos.z += (Random.Range(-maxOffset, maxOffset));
            }

            g.transform.position = startPos;
        }
#endif
    }

    public void OffsetRotations()
    {
#if UNITY_EDITOR
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject g in selectedObjects)
        {
            Vector3 startRot = g.transform.eulerAngles;

            // X Axis
            if (offsetOnXAxis)
            {
                startRot.x += (Random.Range(-maxOffset, maxOffset));
            }

            // Y Axis
            if (offsetOnYAxis)
            {
                startRot.y += (Random.Range(-maxOffset, maxOffset));
            }

            // Z Axis
            if (offsetOnZAxis)
            {
                startRot.z += (Random.Range(-maxOffset, maxOffset));
            }

            g.transform.eulerAngles = startRot;
        }
#endif
    }
}