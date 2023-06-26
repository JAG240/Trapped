using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private RoomManager roomManager;
    private List<Task> taskList = new List<Task>();
    private List<ChopTable> chopTables = new List<ChopTable>();
    private List<Hiding> hidingList = new List<Hiding>();
    private List<Lantern> lanterns = new List<Lantern>();
    private bool registered = false;

    [field: SerializeField] public bool agentAccessible { get; private set; } = false;
    public List<Door> doorList = new List<Door>();
    [field:SerializeField] public int visitCount { get; private set; }
    public PlayerAudio.footstepSource audioType = PlayerAudio.footstepSource.wood;

    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        roomManager.AddRoom(this);

        CheckToRegisterRoom();

        visitCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            visitCount++;
            roomManager.UpdatePlayerCurrentRoom(this);
            other.GetComponentInParent<PlayerAudio>().source = audioType;
            return;
        }

        if (other.transform.tag == "Killer")
            roomManager.UpdateKillerCurrentRoom(this);
    }

    public void AddRoomItem<T>(T item)
    {
        if (item as ChopTable)
        {
            ChopTable table = item as ChopTable;

            if (!chopTables.Contains(table))
                chopTables.Add(table);
            else
                Debug.LogError($"{table.name} already added to chop table");
        }
        else if(item as Lantern)
        {
            Lantern lantern = item as Lantern;

            if (!lanterns.Contains(lantern))
                lanterns.Add(lantern);
            else
                Debug.LogError($"{lantern.name} already added to task list");
        }
        else if(item as Task)
        {
            Task task = item as Task;

            if (!taskList.Contains(task))
                taskList.Add(task);
            else
                Debug.LogError($"{task.name} already added to task list");
        }
        else if(item as Hiding)
        {
            Hiding hiding = item as Hiding;

            if (!hidingList.Contains(hiding))
                hidingList.Add(hiding);
            else
                Debug.LogError($"{hiding.name} already added to hiding list");
        }
        else
        {
            Door door = item as Door;

            if (!doorList.Contains(door))
                doorList.Add(door);
            else
                Debug.LogError($"{door} already added to door list");
        }
    }

    public bool RoomIsLit()
    {
        foreach(Lantern lantern in lanterns)
        {
            if (lantern.lit)
                return true;
        }

        return false;
    }

    public Lantern GetClosestLantern(Vector3 killerPos)
    {
        float closest = float.MaxValue;
        Lantern bestLantern = null;

        foreach(Lantern lantern in lanterns)
        {
            float distance = Vector3.Distance(killerPos, lantern.GetTaskPosition());

            if(distance < closest && lantern.canInteract && lantern.killerInteract)
            {
                closest = distance;
                bestLantern = lantern;
            }
        }

        return bestLantern;
    }

    public Task GetTask()
    {
        foreach(Task task in taskList)
        {
            if (task.onCooldown == false)
                return task;
        }

        return null;
    }

    public ChopTable GetChopTask()
    {
        if(chopTables.Count > 0)
        {
            foreach(ChopTable table in chopTables)
            {
                if (table.HasItem())
                    return table;
            }
        }

        return null;
    }

    public Hiding GetMostVisitedHiding()
    {
        hidingList.Sort(new MostUsedHidingComparer());

        return hidingList[0];
    }

    public Task GetRandomTask()
    {
        List<Task> availableTasks = new List<Task>();
        
        if(taskList.Count < 1 || taskList == null)
            return null;

        foreach(Task task in taskList)
        {
            if (task.onCooldown == false)
                availableTasks.Add(task);
        }

        if (availableTasks.Count < 1)
            return null;

        return availableTasks[Random.Range(0, availableTasks.Count)];
    }

    public Hiding GetRandomHiding()
    {
        if (hidingList.Count <= 0)
            return null;

        return hidingList[Random.Range(0, hidingList.Count)];
    }

    public Hiding GetClosetHiding(Vector3 pos)
    {
        if (hidingList.Count <= 0)
            return null;

        float bestDist = float.MaxValue;
        Hiding bestSpot = null;

        foreach(Hiding spot in hidingList)
        {
            float dist = Vector3.Distance(spot.transform.position, pos);

            if (dist < bestDist)
            {
                bestDist = dist;
                bestSpot = spot;
            }
        }

        return bestSpot;
    }

    public void RefreshAllRooms()
    {
        roomManager.RefreshAllRooms();
    }

    public void CheckToRegisterRoom()
    {
        if (registered)
            return;

        if(agentAccessible && !registered)
        {
            roomManager.RegisterRoom(this);
            registered = true;
            return;
        }

        foreach(Door door in doorList)
        {
            if (door.isLocked)
                continue;

            foreach(Room room in door.roomList)
            {
                if(room.agentAccessible)
                {
                    roomManager.RegisterRoom(this);
                    registered = true;
                    agentAccessible = true;
                    return;
                }
            }
        }

    }
}
