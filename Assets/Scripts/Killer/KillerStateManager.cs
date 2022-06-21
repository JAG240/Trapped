using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerStateManager : MonoBehaviour
{
    private KillerBaseState currentState;
    public NavMeshAgent agent;

    [SerializeField]
    public Transform player;

    [SerializeField]
    public List<Transform> tasks = new List<Transform>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = new Chase();

        currentState.EnterState(this);
        agent.destination = tasks[0].position;
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
