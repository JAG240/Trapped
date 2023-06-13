using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerStateManager : MonoBehaviour
{
    public KillerBaseState currentState { get; private set; }
    public KillerBaseState previousState { get; private set; }
    public KillerBaseState DoTasks { get; private set; } = new DoTasks();
    public KillerBaseState Chase { get; private set; } = new Chase();
    public KillerBaseState Investigate { get; private set; } = new Investigate();
    public KillerBaseState Kill { get; private set; } = new Kill();
    public KillerBaseState Idle { get; private set; } = new Idle();
    public KillerBaseState LightRoom { get; private set; } = new LightRoom();

    public NavMeshAgent agent;
    public Vector3 playerLastSeen = Vector3.zero;
    public Room noisyRoom = null;
    public Transform noise = null;

    [field: SerializeField, Range(0, 100)] public float suspicion { get; private set; }
    [SerializeField] public float killerReach;

    public RoomManager roomManager { get; private set; }
    public Room currentRoom { get; private set; }
    public Detection detection { get; private set; }
    public Transform player { get; private set; }
    public KillerAnimator killerAnimator { get; private set; }
    public SceneManager sceneManager { get; private set; }
    public KillerAudio killerAudio { get; private set; }
    [field: SerializeField] public float walkSpeed { get; private set; } = 3.5f;
    [field: SerializeField] public float runSpeed { get; private set; } = 6f;

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
        roomManager.killerHearing += HeardSound;

        currentState = DoTasks;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(KillerBaseState newState)
    {
        if (newState == currentState)
            return;

        Room keepNoisyRoom = null;
        Transform keepNoise = null;

        if(currentState == Investigate && newState == LightRoom)
        {
            keepNoisyRoom = noisyRoom;
            keepNoise = noise;
        }

        currentState.ExitState(this);
        previousState = GetCurrentState();
        currentState = newState;
        currentState.EnterState(this);

        if(keepNoise && keepNoisyRoom)
        {
            noisyRoom = keepNoisyRoom;
            noise = keepNoise;
        }
    }

    private KillerBaseState GetCurrentState()
    {
        switch (currentState)
        {
            case Kill kill:
                return Kill;
            case DoTasks doTasks:
                return DoTasks;
            case Chase chase:
                return Chase;
            case Investigate investigate:
                return Investigate;
            case LightRoom lightRoom:
                return LightRoom;
            case Idle idle:
                return Idle;
            default:
                return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Door")
        {
            other.transform.parent.GetComponent<Door>().OpenDoor();
        }

        if(other.name == "Room")
        {
            currentRoom = other.GetComponent<Room>();

            if (!currentRoom.RoomIsLit())
                SwitchState(LightRoom);
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

    private void HeardSound(Room audioRoom, Transform audioSource)
    {
        noisyRoom = audioRoom;
        noise = audioSource;
        SwitchState(Investigate);

        Debug.Log($"Killer heard something in {audioRoom.transform.parent.name}");
    }
}
