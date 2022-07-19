using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private CameraController cameraController;
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
    [SerializeField] private float needleSpeed;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraController = player.GetComponentInChildren<CameraController>();
        characterController = player.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        smoke = GetComponentInChildren<ParticleSystem>();
        rpmNeedle = transform.Find("Needle_RPM");
        speedNeedle = transform.Find("Needle_Speedometer");

        if(startIntro)
            EnterCar();
    }

    private void EnterCar()
    {
        characterController.enabled = false;
        playerMovement.enabled = false;
        cameraController.enabled = false;

        player.transform.position = GetSeatPosition();
        player.transform.LookAt(GetLookPosition(), Vector3.up);
    }

    public void ExitCar()
    {
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
