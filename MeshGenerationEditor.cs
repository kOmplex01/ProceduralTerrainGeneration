using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGeneration))]
public class MeshGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeshGeneration script = (MeshGeneration)target;
        if (GUI.changed)
        {
            script.FirstStep();

        }



    }
}
