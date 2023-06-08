using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    public static RoomManager Instance { get { return instance; } }

    [SerializeField] private List<Room> activeRoomList = new List<Room>();
    private List<Room> roomList = new List<Room>();
    public Room playerCurrentRoom { get; private set; }
    public Room killerCurrentRoom { get; private set; }
    public Action<Room, Transform> killerHearing;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void RegisterRoom(Room room)
    {
        if (!activeRoomList.Contains(room))
            activeRoomList.Add(room);
        else
            Debug.LogError($"{room.transform.parent.name} was already added");
    }

    public void AddRoom(Room room)
    {
        if (!roomList.Contains(room))
            roomList.Add(room);
        else
            Debug.LogError($"{room.transform.name} was already added");
    }

    public void RefreshAllRooms()
    {
        foreach(Room room in roomList)
        {
            room.CheckToRegisterRoom();
        }
    }

    public void RemoveRoom(Room room)
    {
        activeRoomList.Remove(room);
    }

    public void UpdatePlayerCurrentRoom(Room room)
    {
        playerCurrentRoom = room;
    }

    public void UpdateKillerCurrentRoom(Room room)
    {
        killerCurrentRoom = room;
    }

    public bool KillerInRoomAudio(GameObject audioSource)
    {
        Room audioRoom = GetRoom(audioSource.transform);

        if(!audioRoom)
        {
            Debug.Log($"Audio room cannot be found for {audioSource.name}");
            return false;
        }

        if(killerCurrentRoom == audioRoom)
        {
            Debug.Log("Killer heard you!");
            return true;
        }

        return false;
    }

    public bool KillerInConnectedRoomAudio(GameObject audioSource)
    {
        Room audioRoom = GetRoom(audioSource.transform);

        if (!audioRoom)
        {
            Debug.Log($"Audio room cannot be found for {audioSource.name}");
            return false;
        }

        foreach (Door door in audioRoom.doorList)
        {
            foreach(Room room in door.roomList)
            {
                if (audioRoom != room && killerCurrentRoom == room)
                {
                    Debug.Log($"Killer Heard you!");
                    return true;
                }
            }
        }

        return false;
    }

    public bool GlobalAudioAlert(GameObject audioSource)
    {
        Room audioRoom = GetRoom(audioSource.transform);

        if (!audioRoom)
        {
            Debug.Log($"Audio room cannot be found for {audioSource.name}");
            return false;
        }

        killerHearing?.Invoke(audioRoom, audioSource.transform);

        return true;
    }

    public Room GetRoom(Transform pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos.position, 0.2f, LayerMask.GetMask("Room"));
        return hits.Count() > 0 ? hits[0].GetComponent<Room>() : null;
    }

    public Room GetMostVisitedRoom()
    {
        activeRoomList.Sort(new MostVistedRoomComparer());

        return activeRoomList[0];
    }

    public Room GetRandomRoom()
    {
        return activeRoomList[UnityEngine.Random.Range(0, activeRoomList.Count)];
    }

    public Task GetRandomTask()
    {
        return GetRandomRoom().GetRandomTask();
    }

    public List<Room> GetConnectedRooms(Door door)
    {
        List<Room> rooms = new List<Room>();

        foreach(Room room in activeRoomList)
        {
            if (room.doorList.Contains(door))
                rooms.Add(room);
        }

        return rooms.Count > 0 ? rooms : null;
    }

    public List<Room> GetConnectedRooms(Room room)
    {
        List<Room> rooms = new List<Room>();

        foreach(Door door in room.doorList)
        {
            List<Room> doorRooms = door.roomList;

            foreach(Room doorRoom in doorRooms)
            {
                if (room != doorRoom)
                    rooms.Add(doorRoom);
            }
        }

        rooms.Sort(new MostVistedRoomComparer());

        return rooms.Count > 0 ? rooms : null;
    }

    public Task GetTaskCloseToRoom(Room room)
    {
        List<Room> rooms = GetConnectedRooms(room);

        foreach(Room r in rooms)
        {
            Task task = r.GetRandomTask();

            if (task)
                return task;
        }

        return null;
    }

    public Task GetTaskInRoom(Room room)
    {
        return room.GetRandomTask();
    }
}
