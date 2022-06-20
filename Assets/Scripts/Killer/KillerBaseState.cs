using UnityEngine;

public abstract class KillerBaseState 
{
    public abstract void EnterState(KillerStateManager Manager);
    public abstract void UpdateState(KillerStateManager Manager);
    public abstract void ExitState(KillerStateManager Manager);
}
