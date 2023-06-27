using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : KillerBaseState
{
    private Hiding checkHiding = null;
    private bool checking = false;

    public override void EnterState(KillerStateManager Manager)
    {
        Manager.agent.speed = Manager.runSpeed;

        if(Manager.playerLastSeen != Vector3.zero)
        {
            if (CheckForClosetEscape(Manager))
                return;

            Manager.agent.SetDestination(Manager.playerLastSeen);
            return;
        }

        if(Manager.noise)
        {
            Manager.agent.SetDestination(Manager.noise.position);
        }
    }

    public override void ExitState(KillerStateManager Manager)
    {
        Manager.noise = null;
        Manager.noisyRoom = null;

        Manager.playerLastSeen = Vector3.zero;
        Manager.agent.speed = Manager.walkSpeed;

        Manager.killerAnimator.SetCheck(false);

        checkHiding = null;
        checking = false;
    }

    public override void UpdateState(KillerStateManager Manager)
    {
        if (Manager.agent.pathPending || Manager.agent.hasPath)
            return;

        if(Manager.playerLastSeen != Vector3.zero)
            Manager.playerLastSeen = Vector3.zero;

        if(Manager.noise)
            Manager.noise = null;

        if(Manager.noisyRoom && !checking || checkHiding)
            CheckHiding(Manager);

        if (Manager.noisyRoom == null && Manager.currentState==this)
            Manager.SwitchState(Manager.DoTasks);
    }

    private bool CheckForClosetEscape(KillerStateManager Manager)
    {
        Collider[] hits = Physics.OverlapSphere(Manager.playerLastSeen, 1.5f, LayerMask.GetMask("Room"));

        if (hits.Length <= 0)
            return false;

        foreach(Collider hit in hits)
        {
            Room lastRoom = hit.GetComponent<Room>();

            if (lastRoom != Manager.roomManager.playerCurrentRoom)
                continue;

            if(lastRoom != null)
            {
                checkHiding = lastRoom.GetClosetHiding(Manager.playerLastSeen);

                if (!checkHiding)
                    return false;

                Manager.agent.SetDestination(checkHiding.GetCheckPosition());

                return true;
            }
        }

        return false;
    }

    private void CheckHiding(KillerStateManager Manager)
    {
        if (Manager.agent.speed == Manager.runSpeed)
            Manager.agent.speed = Manager.walkSpeed;

        if(checkHiding != null && !checking)
        {
            checking = true;
            Manager.lookAt(checkHiding.transform.position);
            Manager.StartCoroutine(PlayCheckAnim(Manager));
            return;
        }

        Hiding hidingSpot = Manager.noisyRoom.GetRandomHiding();
        checkHiding = hidingSpot;
        Manager.agent.SetDestination(checkHiding.GetCheckPosition());
    }

    private IEnumerator PlayCheckAnim(KillerStateManager Manager)
    {
        Manager.killerAnimator.SetCheck(true);
        Closet closet = checkHiding as Closet;

        if (closet)
            closet.OpenDoors(true);

        if (closet.HasPlayer())
            Manager.SwitchState(Manager.Kill);

        yield return new WaitForSeconds(1.5f);
        Manager.killerAnimator.SetCheck(false);

        if (closet.HasPlayer())
            yield break;

        if (closet)
            closet.OpenDoors(false);

        checkHiding = null;
        checking = false;

        //This is temporary! 
        Manager.noisyRoom = null;
    }
}
