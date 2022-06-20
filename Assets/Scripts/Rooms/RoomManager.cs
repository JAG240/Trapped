using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    public static RoomManager Instance { get { return instance; } }

    private HashSet<Room> roomList = new HashSet<Room>();

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
        roomList.Add(room);
    }

    public void RemoveRoom(Room room)
    {
        roomList.Remove(room);
    }
}
