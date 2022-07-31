using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneManager sceneManager = (SceneManager)target;
        if (GUILayout.Button("Start Game"))
        {
            sceneManager.StartGame();
        }
    }
}
