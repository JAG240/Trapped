using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        Manager.sceneManager.KillPlayer(Manager);
        Manager.agent.enabled = false;
        Manager.killerAnimator.SetChop(true);
        Manager.killerAudio.PlayAngryPig();
    }

    public override void ExitState(KillerStateManager Manager)
    {
        Manager.agent.enabled = true;
        Manager.killerAnimator.SetChop(false);
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        //Left blank intentionally
    }
}
