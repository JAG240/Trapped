using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Vector3 seatPosOffset;
    [SerializeField] private Vector3 lookPosOffset;
    [SerializeField] private Vector3 exitOffSet;
    [SerializeField] private AudioClip stallNoise;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    [field: SerializeField] public float needleSpeed { get; private set; }

    private GameObject player;
    private PlayerAudio playerAudio;
    private AudioSource audioSource;
    private AudioLowPassFilter audioLowPassFilter;
    private ParticleSystem smoke;
    private Transform rpmNeedle;
    private Transform speedNeedle;
    private SceneManager sceneManager;
    private float defaultVolume;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerAudio = player.GetComponentInChildren<PlayerAudio>();
        audioSource = GetComponent<AudioSource>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        smoke = GetComponentInChildren<ParticleSystem>();
        rpmNeedle = transform.Find("Needle_RPM");
        speedNeedle = transform.Find("Needle_Speedometer");
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    private void Start()
    {
        sceneManager.playerPrefsUpdated += UpdateVolume;
        defaultVolume = audioSource.volume;
    }

    public void EnterCar()
    {
        sceneManager.EnterCar(this);
    }

    public void ExitCar()
    {
        StartCoroutine(PlayDoorFX());
        sceneManager.ExitCar(this);
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

    public Vector3 GetSeatPosition()
    {
        return transform.position + seatPosOffset;
    }

    public Vector3 GetLookPosition()
    {
        return transform.position + lookPosOffset;
    }

    public Vector3 GetExitPosition()
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

    private void UpdateVolume()
    {
        float volume = PlayerPrefs.GetFloat("main_volume");
        audioSource.volume = defaultVolume * volume;
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
