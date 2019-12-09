using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OfficeLayoutGenerator))]
public class OfficeLayoutGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            OfficeLayoutGenerator script = (OfficeLayoutGenerator)target;
            script.StartNewGeneration(script.eventCode);
        }
    }
}
