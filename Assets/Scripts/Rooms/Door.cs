using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IPeekable, IStateComparable
{
    public bool open { get; private set; }
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float peekAngle = 5f;
    private GameObject door;
    [SerializeField] private bool isSus = false;

    private void Start()
    {
        door = transform.Find("door").gameObject;
        open = false;

        Collider[] rooms = Physics.OverlapSphere(transform.position, 3, LayerMask.GetMask("Room"));
        foreach(Collider room in rooms)
        {
            room.GetComponent<Room>().AddDoor(this);
        }
    }

    public void Interact(GameObject player)
    {
        if (!open)
        {
            isSus = true;
            OpenDoor();
        }
        else
        {
            isSus = false;
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (open)
            return;

        open = true;
        RotateDoor(90f);
    }

    public void CloseDoor()
    {
        if (!open)
            return;

        open = false;
        RotateDoor(-90f);
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

    public bool StateChanged()
    {
        if(isSus)
        {
            isSus = false;
            return true;
        }

        return false;
    }
}