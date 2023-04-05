using UnityEngine;

[ExecuteInEditMode]
public class UVTextureResizing : MonoBehaviour
{

    public float scaleZ = 5.0f;
    public float scaleX = 5.0f;

    private float _scaleZ = 0f;
    private float _scaleX = 0f;

    Material mat;

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / scaleX, transform.localScale.z / scaleZ);
        Application.quitting += DestroyThis;
    }

    private void OnDrawGizmos()
    {
        if ((transform.hasChanged || scaleX != _scaleX || scaleZ != _scaleZ) && Application.isEditor && !Application.isPlaying)
        {
            _scaleZ = scaleZ;
            _scaleX = scaleX;

            GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x / scaleX, transform.localScale.z / scaleZ);
            transform.hasChanged = false;
        }
    }

    private void DestroyThis()
    {
        DestroyImmediate(this);
    }
}