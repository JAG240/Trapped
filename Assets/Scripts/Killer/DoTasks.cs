using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoTasks : KillerBaseState
{
    private bool taskAssigned;
    private bool doingTask;
    private Task prevTask;

    public override void EnterState(KillerStateManager Manager)
    {
        taskAssigned = false;
        doingTask = false;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if (!Manager.agent.pathPending && !Manager.agent.hasPath && !doingTask && taskAssigned)
        {
            Manager.StartCoroutine(WaitForTask(prevTask.taskTime, Manager));
            Manager.StartCoroutine(LookTo(Manager, prevTask.transform.position, 0.2f));
        }

        if (taskAssigned)
            return;

        taskAssigned = true;
        Task newTask = null;

        switch (Manager.suspicion)
        {
            case float s when s <= 30:
                newTask = Manager.roomManager.GetTaskCloseToRoom(Manager.roomManager.GetMostVisitedRoom());
                break;
            case float s when s <= 50:
                newTask = Manager.roomManager.GetTaskCloseToRoom(Manager.roomManager.playerCurrentRoom);
                break;
            case float s when s <= 100:
                newTask = Manager.roomManager.GetTaskInRoom(Manager.roomManager.playerCurrentRoom);
                break;
            default:
                newTask = Manager.roomManager.GetRandomTask();
                break;
        }

        if (!newTask)
            newTask = Manager.roomManager.GetRandomTask();

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
        if (doingTask)
            prevTask.CancelTask();

        Manager.StopAllCoroutines();
        Manager.killerAnimator.SetChop(false);

        taskAssigned = false;
        doingTask = false;
    }

    private IEnumerator WaitForTask(float timer, KillerStateManager Manager)
    {
        doingTask = true;
        yield return new WaitForSeconds(timer);
        taskAssigned = false;
        prevTask.CompleteTask();
        doingTask = false;
        Manager.killerAnimator.SetChop(false);
    }

    private IEnumerator LookTo(KillerStateManager manager, Vector3 target, float speed)
    {
        float t = 0f;

        Vector3 lookDir = target - manager.transform.position;
        lookDir.y = 0;
        Quaternion end = Quaternion.LookRotation(lookDir);
        Quaternion start = new Quaternion(manager.transform.rotation.x, manager.transform.rotation.y, manager.transform.rotation.z, manager.transform.rotation.w);

        while(t < speed)
        {
            manager.transform.rotation = Quaternion.Slerp(start, end, t/speed);
            yield return null;
            t += Time.deltaTime;
        }

        manager.transform.rotation = end;
        manager.killerAnimator.SetChop(true);
    }
}
