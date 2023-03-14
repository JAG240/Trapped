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
    public KillerBaseState Kill { get; private set; } = new Kill();
    public KillerBaseState Idle { get; private set; } = new Idle();

    public NavMeshAgent agent;
    public Vector3 playerLastSeen = Vector3.zero;

    [field: SerializeField, Range(0, 100)] public float suspicion { get; private set; }
    [SerializeField] public float killerReach;

    public RoomManager roomManager { get; private set; }
    public Detection detection { get; private set; }
    public Transform player { get; private set; }
    public KillerAnimator killerAnimator { get; private set; }
    public SceneManager sceneManager { get; private set; }
    public KillerAudio killerAudio { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        killerAnimator = GetComponent<KillerAnimator>();
        detection = GetComponent<Detection>();
        killerAudio = GetComponentInChildren<KillerAudio>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        player = GameObject.Find("PlayerCapsule").transform;

        suspicion = 0f;

        detection.detectedObject += seenObject;
        sceneManager.introKill += IntroWarp;
        sceneManager.resetLevel += ResetLevel;

        currentState = Idle;
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
            other.transform.parent.GetComponent<Door>().OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Door")
        {
            other.transform.parent.GetComponent<Door>().CloseDoor();
        }
    }

    public void lookAt(Vector3 target)
    {
        Vector3 lookDir = target - transform.position;
        lookDir.y = 0;
        Quaternion end = Quaternion.LookRotation(lookDir);
        transform.rotation = end;
    }

    private void IntroWarp()
    {
        agent.Warp(player.position - (Camera.main.transform.forward * 2.5f));
        lookAt(player.position);
    }

    private void ResetLevel()
    {
        agent.Warp(sceneManager.killerRespawnPoint);
        SwitchState(DoTasks);
    }

    //Can be passed non-player object
    private void seenObject(GameObject obj)
    {
        //Possible to set non-player object as last seen location
        if (!obj)
        {
            SwitchState(Investigate);
            playerLastSeen = new Vector3(player.position.x, player.position.y, player.position.z);
            return;
        }

        if (obj.name == "PlayerCapsule")
        {
            player = obj.transform;
            SwitchState(Chase);
        }
        else
            Debug.Log($"{obj.name} seen!");
    }
}
