using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Combine Meshes"))
        {
            MeshCombiner mc = (MeshCombiner)target;
            mc.CombineMesh();
        }
    }
}
