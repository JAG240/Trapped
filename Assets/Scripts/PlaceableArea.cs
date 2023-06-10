using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableArea : MonoBehaviour
{
    [SerializeField] private Transform placePos;
    [SerializeField] private List<SpecialTransform> specialTransforms = new List<SpecialTransform>();
    public GameObject placedObject { private set; get; }
    public bool needsChopped { private set; get; } = false;

    private void OnTriggerEnter(Collider other)
    {
        Transform transPos = SpecialTransformCheck(other.name);

        if(other.GetComponent<BodyParts>() && placedObject == null)
        {
            other.transform.position = transPos.position;
            other.transform.rotation = transPos.rotation;
            other.attachedRigidbody.isKinematic = true;
            other.enabled = false;
            placedObject = other.gameObject;
            needsChopped = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }

    private Transform SpecialTransformCheck(string name)
    {
        foreach(SpecialTransform transform in specialTransforms)
        {
            if (name.Contains(transform.Name))
                return transform.Pos;
        }

        return placePos;
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

    public void ClearSpace()
    {
        placedObject = null;
        needsChopped = false;
        gameObject.layer = 0;
    }

    public void CompleteChop()
    {
        needsChopped = false;
        placedObject.GetComponent<BodyParts>().GetChopped(this);
    }
}

[Serializable]
public class SpecialTransform
{
    [field: SerializeField] public Transform Pos { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
}