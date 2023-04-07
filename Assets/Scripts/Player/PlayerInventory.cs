using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private Transform LeftHandPos;
    [SerializeField] private float throwForce;
    private List<string> keys = new List<string>();
    private GameObject heldItem = null;
    private Rigidbody objBody = null;
    private Collider objCollider = null;

    public bool HandsFull()
    {
        if (heldItem)
            return true;

        return false;
    }

    public void PutInHands(GameObject obj)
    {
        heldItem = obj;

        obj.transform.position = rightHandPos.position;

        objBody = obj.GetComponent<Rigidbody>();
        objCollider = obj.GetComponent<Collider>();

        if(objBody)
            objBody.isKinematic = true;

        if (objCollider)
            objCollider.enabled = false;

        obj.transform.position = rightHandPos.position;
        obj.transform.rotation = rightHandPos.rotation;
        obj.transform.parent = rightHandPos;
    }

    public void DropItem()
    {
        heldItem.transform.parent = null;
        heldItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

        if (objBody)
            objBody.isKinematic = false;

        if (objCollider)
            objCollider.enabled = true;

        heldItem = null;
        objCollider = null;
        objBody = null;
    }

    public void ThrowItem()
    {
        Rigidbody body = objBody;

        DropItem();

        body.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }

    public bool ConsumeKey(string key)
    {
        if(keys.Contains(key))
        {
            keys.Remove(key);
            return true;
        }

        return false;
    }

    public void AddKey(string key)
    {
        keys.Add(key);
    }
}
