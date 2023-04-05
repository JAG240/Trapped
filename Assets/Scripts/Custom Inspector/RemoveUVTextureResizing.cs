using UnityEngine;
using UnityEditor;

public class RemoveUVTextureResizing : MonoBehaviour
{
    public void RemoveScripts()
    {
        UVTextureResizing[] scripts = GetComponentsInChildren<UVTextureResizing>();

        foreach(UVTextureResizing script in scripts)
        {
            DestroyImmediate(script);
        }

        Debug.Log("All Texture Resizing Removed");
    }
}

[CustomEditor(typeof(RemoveUVTextureResizing))]
public class CustomEditorRemoveUVTextureResizing : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RemoveUVTextureResizing removeScript = (RemoveUVTextureResizing)target;
        if (GUILayout.Button("Remove All Texture Resizing Scripts"))
        {
            removeScript.RemoveScripts();
        }
    }
}