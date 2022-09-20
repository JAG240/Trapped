using UnityEngine;

[ExecuteInEditMode]
public class UVTextureResizing : MonoBehaviour
{

    public float scaleFactor = 5.0f;
    Material mat;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Start" + gameObject.name);
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / scaleFactor, transform.localScale.z / scaleFactor);
    }

    void Update()
    {

        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("The transform has changed!");
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / scaleFactor, transform.localScale.z / scaleFactor);
            transform.hasChanged = false;
        }

    }
}