using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IPeekable
{
    [SerializeField] private bool open = false;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float peekAngle = 5f;
    private GameObject door;

    private void Start()
    {
        door = transform.Find("door").gameObject;
    }

    public void Interact(GameObject player)
    {
        if(!open)
        {
            open = true;
            RotateDoor(90f);
        }
        else
        {
            open = false;
            RotateDoor(-90f);
        }
    }
    public void Peek(GameObject obj, bool state)
    {
        if (state)
            RotateDoor(peekAngle);
        else
            RotateDoor(-peekAngle);
    }

    private void RotateDoor(float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
        door.transform.localPosition = rot * (door.transform.localPosition - pivotPoint.localPosition) + pivotPoint.localPosition;
        door.transform.localRotation = rot * door.transform.localRotation;
    }

}
