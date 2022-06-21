using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private RoomManager roomManager;
    private List<Task> taskList = new List<Task>();
    private List<Hiding> hidingList = new List<Hiding>();
    public int visitCount { get; private set; }

    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        roomManager.AddRoom(this);
        visitCount = 0;
    }

    public void AddTask(Task task)
    {
        if (!taskList.Contains(task))
            taskList.Add(task);
        else
            Debug.LogError($"{task.name} already added to task list");
    }

    public void RemoveTask(Task task)
    {
        taskList.Remove(task);
    }

    public void AddHiding(Hiding hiding)
    {
        if (!hidingList.Contains(hiding))
            hidingList.Add(hiding);
        else
            Debug.LogError($"{hiding.name} already added to hiding list");
    }

    public void RemoveHiding(Hiding hiding)
    {
        hidingList.Remove(hiding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            visitCount++;
    }

    public Task GetTask()
    {
        Task bestTask = taskList[0];

        foreach(Task task in taskList)
        {
            if (task.onCooldown == false)
                return task;
        }

        return null;
    }

    public Hiding GetMostVisitedHiding()
    {
        hidingList.Sort(new MostUsedHidingComparer());

        return hidingList[0];
    }
}
