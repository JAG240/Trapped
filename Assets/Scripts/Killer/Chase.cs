using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        lookAt(Manager.player.position, Manager);
        Manager.agent.destination = Manager.player.position;
    }

    public override void ExitState(KillerStateManager Manager)
    {

    }

    private void lookAt(Vector3 target, KillerStateManager manager)
    {
        Vector3 lookDir = target - manager.transform.position;
        lookDir.y = 0;
        Quaternion end = Quaternion.LookRotation(lookDir);
        manager.transform.rotation = end;
    }
}
