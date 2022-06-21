using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    public static RoomManager Instance { get { return instance; } }

    private List<Room> roomList = new List<Room>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public void AddRoom(Room room)
    {
        if (!roomList.Contains(room))
            roomList.Add(room);
        else
            Debug.LogError($"{room.transform.name} was already added");
    }

    public void RemoveRoom(Room room)
    {
        roomList.Remove(room);
    }

    public Room GetMostVisitedRoom()
    {
        roomList.Sort(new MostVistedRoomComparer());

        return roomList[0];
    }
}
