using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        Manager.agent.speed = Manager.runSpeed;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        Manager.lookAt(Manager.player.position);
        Manager.agent.destination = Manager.player.position;

        if(Vector3.Distance(Manager.transform.position, Manager.player.position) < Manager.killerReach)
        {
            Manager.SwitchState(Manager.Kill);
        }
    }

    public override void ExitState(KillerStateManager Manager)
    {
        Manager.agent.speed = Manager.walkSpeed;
        Manager.agent.ResetPath();
    }
}
