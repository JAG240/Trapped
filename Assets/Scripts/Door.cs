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
<<<<<<< Updated upstream
<<<<<<< HEAD
        if (!open)
            OpenDoor();
        else
            CloseDoor();
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

=======
        if(!open)
        {
            open = true;
            RotateDoor(90f);
        }
=======
        if (!open)
            OpenDoor();
>>>>>>> Stashed changes
        else
            CloseDoor();
    }
<<<<<<< Updated upstream
>>>>>>> 8e2e0b12c21d11e0b8e06251c74ca39e65aaf7d4
=======

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

>>>>>>> Stashed changes
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
