using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerStateManager : MonoBehaviour
{
    private KillerBaseState currentState;
    private KillerBaseState DoTasks = new DoTasks();
    private KillerBaseState Chase = new Chase();

    public NavMeshAgent agent;
    public Transform player;

    [field: SerializeField, Range(0, 100)] public float suspicion { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        suspicion = 0f;

        currentState = DoTasks;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Door")
        {
            other.transform.root.GetComponent<Door>().OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Door")
        {
            other.transform.root.GetComponent<Door>().CloseDoor();
        }
    }
}
