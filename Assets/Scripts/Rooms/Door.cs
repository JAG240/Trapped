using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable, IPeekable, IStateComparable
{

    [Header("Build settings")]
    [SerializeField] private bool activeDoor = true;
    [SerializeField] private bool registerDoor = true;
    [field: SerializeField] public bool isLocked { get; private set; } = false;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;

    [Header("Runtime Vars")]
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float peekAngle = 5f;
    [SerializeField] private bool isSus = false;
    [SerializeField] private NavMeshObstacle navMeshLock;

    private AudioSource audioSource;
    private List<Lock> locks;

    public bool open { get; private set; } = false;
    private GameObject door;
    public List<Room> roomList { get; private set; } = new List<Room>();

    private void Start()
    {
        door = transform.Find("door").gameObject;
        audioSource = GetComponent<AudioSource>();
        locks = new List<Lock>(gameObject.GetComponentsInChildren<Lock>());

        if (locks.Count <= 0)
            navMeshLock.enabled = false;
        else
            isLocked = true;

        if (!registerDoor)
            return;

        Collider[] rooms = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Room"));
        foreach(Collider room in rooms)
        {
            Room roomScript = room.GetComponent<Room>();
            roomList.Add(roomScript);
            roomScript.AddRoomItem<Door>(this);
        }
    }

    public void Interact(GameObject player)
    {
        if (!CheckLocks(player.GetComponent<PlayerInventory>()))
        {
            //tell the player they do not have the correct key
            return;
        }

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
        if (open || !activeDoor)
            return;

        audioSource.clip = openDoor;
        audioSource.Play();
        open = true;
        RotateDoor(90f);
    }

    public void CloseDoor()
    {
        if (!open || !activeDoor)
            return;

        audioSource.clip = closeDoor;
        audioSource.Play();
        open = false;
        RotateDoor(-90f);
    }

    public void Peek(GameObject obj, bool state)
    {
        if (isLocked)
            return;

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

    private bool CheckLocks(PlayerInventory playerInventory)
    {
        if (!isLocked)
            return true;

        if (isLocked && locks.Count <= 0)
        {
            isLocked = false;

            foreach(Room room in roomList)
            {
                room.CheckToRegisterRoom();
            }

            if (roomList.Count > 0)
                roomList[0].RefreshAllRooms();

            return true;
        }

        for(int i = 0; i < locks.Count; i++)
        {
            if (locks[i].Unlock(playerInventory))
            {
                locks.RemoveAt(i);

                if(locks.Count <= 0 && navMeshLock)
                {
                    navMeshLock.enabled = false;
                }

                break;
            }
        }

        if (locks.Count <= 0)
        {
            CheckLocks(playerInventory);
        }

        return false;
    }
}