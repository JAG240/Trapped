using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    private SceneManager sceneManager;
    private Vector3 resetPos;
    private Quaternion resetRot;
    private Rigidbody rigidBody;
    private Collider col;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        resetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        resetRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.resetLevel += ResetObject;
    }

    private void OnDestroy()
    {
        if (sceneManager == null)
            return;

        sceneManager.resetLevel -= ResetObject;
    }

    private void ResetObject()
    {
        transform.parent = null;
        transform.position = resetPos;
        transform.rotation = resetRot;
        rigidBody.isKinematic = false;
        col.enabled = true;
    }
}
