using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : KillerBaseState
{
    public override void EnterState(KillerStateManager Manager)
    {
        Debug.Log($"You have been killed!");

        Manager.sceneManager.KillPlayer(Manager);
        Manager.agent.enabled = false;
        Manager.killerAnimator.SetChop(true);
        Manager.killerAudio.PlayAngryPig();

        //Stop killer
        //Disable player input
        //Rotate camera to killer
        //Play killer animation
        //RESET LEVEL
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
