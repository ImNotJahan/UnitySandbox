using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LSystems))]
public class LSystemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LSystems LSystem = (LSystems)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            LSystem.Start();
        }
    }
}
