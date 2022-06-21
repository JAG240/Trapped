using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoTasks : KillerBaseState
{
    bool doingTask;
    Transform prevTask;

    public override void EnterState(KillerStateManager Manager)
    {
        doingTask = false;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if (!Manager.agent.pathPending && !Manager.agent.hasPath)
            doingTask = false;


        if (!doingTask)
        {
            doingTask = true;

            List<Transform> choices = new List<Transform>(Manager.tasks);
            choices.Remove(prevTask);

            Transform task = choices[Random.Range(0, choices.Count)];
            prevTask = task;
            Manager.agent.destination = task.position;
        }
    }

    public override void ExitState(KillerStateManager Manager)
    {
        throw new System.NotImplementedException();
    }   
}
