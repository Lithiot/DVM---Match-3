using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Controller))]
public class Controller_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Controller script = (Controller)target;
        if (GUILayout.Button("Create grid"))
        {
            script.GeneratePrefabGrid();
        }
        if (GUILayout.Button("Clear grid"))
        {
            script.ClearGrid();
        }
    }
}
