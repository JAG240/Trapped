using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IPeekable, IStateComparable
{
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float peekAngle = 5f;
    [SerializeField] private bool isSus = false;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;
    private AudioSource audioSource;

    public bool open { get; private set; } = false;
    private GameObject door;
    public List<Room> roomList { get; private set; } = new List<Room>();

    private void Start()
    {
        door = transform.Find("door").gameObject;
        audioSource = GetComponent<AudioSource>();

        Collider[] rooms = Physics.OverlapSphere(transform.position, 3, LayerMask.GetMask("Room"));
        foreach(Collider room in rooms)
        {
            Room roomScript = room.GetComponent<Room>();
            roomList.Add(roomScript);
            roomScript.AddRoomItem<Door>(this);
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

        audioSource.clip = openDoor;
        audioSource.Play();
        open = true;
        RotateDoor(90f);
    }

    public void CloseDoor()
    {
        if (!open)
            return;

        audioSource.clip = closeDoor;
        audioSource.Play();
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