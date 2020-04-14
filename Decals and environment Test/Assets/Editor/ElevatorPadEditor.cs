using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelZeroElevatorPad))]
public class ElevatorPadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test Fail Anim"))
        {
            LevelZeroElevatorPad ep = (LevelZeroElevatorPad)target;
            ep.StartCoroutine(ep.PadFailAnimation());
        }

        if (GUILayout.Button("Test Success Anim"))
        {
            LevelZeroElevatorPad ep = (LevelZeroElevatorPad)target;
            ep.StartCoroutine(ep.PadSuccessAnimation());
        }
    }
}
