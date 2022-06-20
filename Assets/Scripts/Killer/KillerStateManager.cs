using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerStateManager : MonoBehaviour
{
    private KillerBaseState currentState;

    void Start()
    {
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(KillerBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.ExitState(this);
    }
}
