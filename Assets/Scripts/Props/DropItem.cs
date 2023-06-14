using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IChopInteraction
{
    [SerializeField] private GameObject droppedObject;
    [SerializeField] private Transform dropPos;
    private bool dropped = false;

    public void ChopInteract(PlaceableArea area)
    {
        if (dropped)
            return;

        Instantiate(droppedObject, dropPos);
        dropped = true;
    }
}
