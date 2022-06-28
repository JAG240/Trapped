using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoTasks : KillerBaseState
{
    private bool taskAssigned;
    private bool doingTask;
    private Task prevTask;
    private RoomManager roomManager;

    public override void EnterState(KillerStateManager Manager)
    {
        taskAssigned = false;
        doingTask = false;

        if (!roomManager)
            roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if (!Manager.agent.pathPending && !Manager.agent.hasPath && !doingTask && taskAssigned)
            Manager.StartCoroutine(WaitForTask(prevTask.taskTime));

        if (taskAssigned)
            return;

        taskAssigned = true;
        Task newTask = null;

        switch (Manager.suspicion)
        {
            case float s when s <= 30:
                newTask = roomManager.GetTaskCloseToRoom(roomManager.GetMostVisitedRoom());
                break;
            case float s when s <= 50:
                newTask = roomManager.GetTaskCloseToRoom(roomManager.playerCurrentRoom);
                break;
            case float s when s <= 100:
                newTask = roomManager.GetTaskInRoom(roomManager.playerCurrentRoom);
                break;
            default:
                newTask = roomManager.GetRandomTask();
                break;
        }

        if (!newTask)
            newTask = roomManager.GetRandomTask();

        if (!newTask || newTask == prevTask)
        {
            taskAssigned = false;
            return;
        }

        Manager.agent.destination = newTask.GetTaskPosition();
        prevTask = newTask;

    }

    public override void ExitState(KillerStateManager Manager)
    {
        taskAssigned = false;
        doingTask = false;
    }

    private IEnumerator WaitForTask(float timer)
    {
        doingTask = true;
        yield return new WaitForSeconds(timer);
        taskAssigned = false;
        prevTask.CompleteTask();
        doingTask = false;
    }
}
