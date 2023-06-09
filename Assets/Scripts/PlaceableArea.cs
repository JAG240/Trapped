using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableArea : MonoBehaviour
{
    [SerializeField] private Transform placePos;
    public GameObject placedObject { private set; get; }
    public bool needsChopped { private set; get; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BodyParts>() && placedObject == null)
        {
            other.transform.position = placePos.position;
            other.transform.rotation = placePos.rotation;
            other.attachedRigidbody.isKinematic = true;
            other.enabled = false;
            placedObject = other.gameObject;
            needsChopped = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }

    public void RemoveFromPlace(GameObject player)
    {
        if (!placedObject)
            return;

        placedObject.GetComponent<IInteractable>().Interact(player);
        placedObject = null;
        needsChopped = false;
        gameObject.layer = 0;
    }

    public void CompleteChop()
    {
        needsChopped = false;
        placedObject.GetComponent<BodyParts>().GetChopped();
    }
}