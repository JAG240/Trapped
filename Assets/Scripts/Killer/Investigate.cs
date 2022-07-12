using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        if (Manager.playerLastSeen != Vector3.zero)
        {
            Manager.agent.SetDestination(Manager.playerLastSeen);
        }
    }

    public override void ExitState(KillerStateManager Manager)
    {
        Manager.playerLastSeen = Vector3.zero;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if(!Manager.agent.pathPending && !Manager.agent.hasPath && Manager.agent.remainingDistance == 0)
        {
            Manager.SwitchState(Manager.DoTasks);
        }
    }


}
