using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        Manager.StopAllCoroutines();
        Manager.agent.enabled = false;
    }

    public override void ExitState(KillerStateManager Manager)
    {
        Manager.agent.enabled = true;
    }

    public override void UpdateState(KillerStateManager Manager)
    {

    }
}
