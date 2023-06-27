using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StreetBuilder))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StreetBuilder streetBuilder = (StreetBuilder)target;
        if(GUILayout.Button("Build Street"))
            streetBuilder.BuildStreet();
    }
}