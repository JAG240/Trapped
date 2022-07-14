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
    [SerializeField] private bool startIntro = false;
    [SerializeField] private Vector3 seatPosOffset;
    [SerializeField] private Vector3 lookPosOffset;
    [SerializeField] private Vector3 exitOffSet;
    [SerializeField] private AudioClip stallNoise;

    void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraController = player.GetComponentInChildren<CameraController>();
        characterController = player.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();

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
