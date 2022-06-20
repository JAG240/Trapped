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
        Manager.agent.destination = Manager.player.position;
    }

    public override void ExitState(KillerStateManager Manager)
    {

    }
}
