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
        currentState = DoTasks;

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
