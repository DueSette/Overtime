using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetPositionOffsetter))]
public class AssetOffsetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Offset Positions"))
        {
            AssetPositionOffsetter apo = (AssetPositionOffsetter)target;
            apo.OffsetPositions();
        }

        if (GUILayout.Button("Offset Rotations"))
        {
            AssetPositionOffsetter apo = (AssetPositionOffsetter)target;
            apo.OffsetRotations();
        }
    }
}