using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerStateManager : MonoBehaviour
{
    private KillerBaseState currentState;
    public KillerBaseState DoTasks { get; private set; } = new DoTasks();
    public KillerBaseState Chase { get; private set; } = new Chase();
    public KillerBaseState Investigate { get; private set; } = new Investigate();

    public NavMeshAgent agent;
    public Vector3 playerLastSeen = Vector3.zero;

    [field: SerializeField, Range(0, 100)] public float suspicion { get; private set; }
    public RoomManager roomManager { get; private set; }
    public Detection detection { get; private set; }
    public Transform player { get; private set; }
    public KillerAnimator killerAnimator { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        killerAnimator = GetComponent<KillerAnimator>();
        detection = GetComponent<Detection>();
        suspicion = 0f;

        detection.detectedObject += seenObject;

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
        currentState.EnterState(this);
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

    private void seenObject(GameObject obj)
    {
        if (!obj)
        {
            SwitchState(Investigate);
            playerLastSeen = new Vector3(player.position.x, player.position.y, player.position.z);
            return;
        }

        if (obj.name == "Player")
        {
            player = obj.transform;
            SwitchState(Chase);
        }
        else
            Debug.Log($"{obj.name} seen!");
    }
}
