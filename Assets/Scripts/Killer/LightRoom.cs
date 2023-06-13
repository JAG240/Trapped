using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRoom : KillerBaseState
{
    private Lantern lantern;
    private bool isFlicking = false;

    public override void EnterState(KillerStateManager Manager)
    {
        lantern = Manager.currentRoom.GetClosestLantern(Manager.transform.position);

        if (lantern == null)
            Manager.SwitchState(Manager.previousState);

        Manager.agent.SetDestination(lantern.GetTaskPosition());
    }

    public override void ExitState(KillerStateManager Manager)
    {
        isFlicking = false;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if (Manager.agent.pathPending || Manager.agent.hasPath)
            return;

        if (!isFlicking && lantern != null)
            Manager.StartCoroutine(PlayFlickAnim(Manager));
    }

    private IEnumerator PlayFlickAnim(KillerStateManager Manager)
    {        
        isFlicking = true;

        Manager.killerAnimator.SetFlick(true);
        yield return new WaitForSeconds(3f);

        BrokenLantern brokenLantern = lantern as BrokenLantern;
        if (brokenLantern)
            brokenLantern.KillerInteract();
        else
            lantern.Interact(null);

        Manager.killerAnimator.SetFlick(false);
        lantern = null;

        Manager.SwitchState(Manager.previousState);
    }
}
