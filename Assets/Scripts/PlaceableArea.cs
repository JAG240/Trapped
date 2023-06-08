using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableArea : MonoBehaviour
{
    [SerializeField] private Transform placePos;
    public GameObject placedObject { private set; get; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BodyParts>())
        {
            other.transform.position = placePos.position;
            other.transform.rotation = placePos.rotation;
            other.attachedRigidbody.isKinematic = true;
            other.enabled = false;
            placedObject = other.gameObject;
        }
    }

    public void RemoveFromPlace(GameObject player)
    {
        if (!placedObject)
            return;

        placedObject.GetComponent<IInteractable>().Interact(player);
    }

    public void CompleteChop()
    {
        placedObject.GetComponent<BodyParts>().GetChopped();
    }
}