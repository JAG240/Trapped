using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private CameraController cameraController;
    private PlayerAudio playerAudio;
    private CharacterController characterController;
    private AudioSource audioSource;
    private AudioLowPassFilter audioLowPassFilter;
    private ParticleSystem smoke;
    private Transform rpmNeedle;
    private Transform speedNeedle;
    [SerializeField] private bool startIntro = false;
    [SerializeField] private Vector3 seatPosOffset;
    [SerializeField] private Vector3 lookPosOffset;
    [SerializeField] private Vector3 exitOffSet;
    [SerializeField] private AudioClip stallNoise;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    [field: SerializeField] public float needleSpeed { get; private set; }

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraController = player.GetComponentInChildren<CameraController>();
        playerAudio = player.GetComponent<PlayerAudio>();
        characterController = player.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        smoke = GetComponentInChildren<ParticleSystem>();
        rpmNeedle = transform.Find("Needle_RPM");
        speedNeedle = transform.Find("Needle_Speedometer");
    }

    void Start()
    {
        if(startIntro)
            EnterCar();
    }

    public void EnterCar()
    {
        characterController.enabled = false;
        playerMovement.enabled = false;
        cameraController.enabled = false;

        player.transform.position = GetSeatPosition();
        player.transform.LookAt(GetLookPosition(), Vector3.up);
    }

    public void ExitCar()
    {
        StartCoroutine(PlayDoorFX());
        characterController.enabled = true;
        playerMovement.enabled = true;
        cameraController.enabled = true;

        player.transform.position = GetExitPosition();
    }

    public void Stall()
    {
        smoke.Play();
        StartCoroutine(DropNeedle(rpmNeedle));
        StartCoroutine(DropNeedle(speedNeedle));
        StartCoroutine(playerAudio.StopIntroMusic(needleSpeed));
        audioSource.Stop();
        audioLowPassFilter.cutoffFrequency = 2000f;
        audioSource.loop = false;
        audioSource.clip = stallNoise;
        audioSource.Play();
    }

    private Vector3 GetSeatPosition()
    {
        return transform.position + seatPosOffset;
    }

    private Vector3 GetLookPosition()
    {
        return transform.position + lookPosOffset;
    }

    private Vector3 GetExitPosition()
    {
        return transform.position + exitOffSet;
    }

    private IEnumerator DropNeedle(Transform needle)
    {
        float totalTime = 0f;
        float startRotation = needle.localRotation.eulerAngles.z;

        while(totalTime < needleSpeed)
        {
            float t = totalTime / needleSpeed;
            float newZ = Mathf.Lerp(startRotation, 360f, t);
            needle.localRotation = Quaternion.Euler(0f, 0f, newZ);
            totalTime += Time.deltaTime;
            yield return null;
        }

        needle.localRotation = Quaternion.identity;
    }

    private IEnumerator PlayDoorFX()
    {
        audioSource.clip = doorOpen;
        audioSource.Play();
        yield return new WaitForSeconds(doorOpen.length);
        audioSource.clip = doorClose;
        audioSource.Play();
    }

#if UNITY_EDITOR

    public bool ShowSeatPosition = false;
    public bool ShowLookPosition = false;
    public bool ShowExitPosition = false;

    private void OnDrawGizmos()
    {
        if (ShowSeatPosition)
            Gizmos.DrawSphere(GetSeatPosition(), 0.2f);

        Gizmos.color = Color.red;
        if (ShowLookPosition)
            Gizmos.DrawSphere(GetLookPosition(), 0.2f);

        Gizmos.color = Color.blue;
        if (ShowExitPosition)
            Gizmos.DrawSphere(GetExitPosition(), 0.2f);
    }

#endif
}
