using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private Transform LeftHandPos;
    [SerializeField] private float throwForce;

    private List<string> keys = new List<string>();

    private Hand rightHeldItem = new Hand();
    private Hand leftHeldItem = new Hand();


    private class Hand
    {
        public GameObject heldItem = null;
        public Rigidbody objBody = null;
        public Collider objCollider = null;
    }

    public bool HandsFull(bool rightHand)
    {
        Hand hand = rightHand ? ref rightHeldItem : ref leftHeldItem;

        if (hand.heldItem)
            return true;

        return false;
    }

    public void PutInHands(GameObject obj, bool rightHand)
    {
        Hand hand = rightHand ? ref rightHeldItem : ref leftHeldItem;

        hand.heldItem = obj;

        hand.objBody = obj.GetComponent<Rigidbody>();
        hand.objCollider = obj.GetComponent<Collider>();

        if(hand.objBody)
            hand.objBody.isKinematic = true;

        if (hand.objCollider)
            hand.objCollider.enabled = false;

        obj.transform.position = rightHand ? rightHandPos.position : LeftHandPos.position;
        obj.transform.rotation = rightHand ? rightHandPos.rotation : LeftHandPos.rotation;
        obj.transform.parent = rightHand ? rightHandPos : LeftHandPos;
    }

    public void DropItem(bool rightHand)
    {
        Hand hand = rightHand ? ref rightHeldItem : ref leftHeldItem;
        
        hand.heldItem.transform.parent = null;
        hand.heldItem.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

        if (hand.objBody)
            hand.objBody.isKinematic = false;

        if (hand.objCollider)
            hand.objCollider.enabled = true;

        hand.heldItem = null;
        hand.objBody = null;
        hand.objCollider = null;
    }

    public void ThrowItem()
    {
        Rigidbody body = rightHeldItem.objBody;

        DropItem(true);

        body.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }

    public bool ConsumeKey(string key)
    {
        if (!leftHeldItem.heldItem)
            return false;

        Key heldKey = leftHeldItem.heldItem.GetComponent<Key>();

        if (heldKey.keyID == key)
        {
            Destroy(leftHeldItem.heldItem);
            return true;
        }

        return false;
    }

    public void ClearHands()
    {
        rightHeldItem.heldItem = null;
        rightHeldItem.objBody = null;
        rightHeldItem.objCollider = null;

        leftHeldItem.heldItem = null;
        leftHeldItem.objBody = null;
        leftHeldItem.objCollider = null;
    }
}
