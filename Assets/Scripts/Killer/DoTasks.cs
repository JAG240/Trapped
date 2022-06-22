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
        if (!taskAssigned)
        {
            taskAssigned = true;

            Task newTask = roomManager.GetRandomTask();

            if (!newTask || newTask == prevTask)
            {
                taskAssigned = false;
                return;
            }

            Manager.agent.destination = newTask.GetTaskPosition();
            prevTask = newTask;
        }

        if (!Manager.agent.pathPending && !Manager.agent.hasPath && !doingTask)
            Manager.StartCoroutine(WaitForTask(prevTask.taskTime));
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
