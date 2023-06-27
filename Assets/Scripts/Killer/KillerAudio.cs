using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KillerAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] float maxDistance = 25f;
    [SerializeField] float minPassVolume = 230f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip step;
    [SerializeField] private AudioClip effortGrunt;
    [SerializeField] private AudioClip fullChop;
    [SerializeField] private AudioClip angryPig;

    [Header("Source References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource killerVoice;

    private AudioLowPassFilter lowPassFilter;
    private Transform player;
    private SceneManager sceneManager;
    private float defaultVolume;
    private float defaultVoiceVolume;

    void Start()
    {
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        player = GameObject.Find("Player").transform;

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.playerPrefsUpdated += UpdateAudioVolume;
        defaultVolume = audioSource.volume;
        defaultVoiceVolume = killerVoice.volume;
    }

    public float Remap(float inMin, float inMax, float outMin, float outMax, float v)
    {
        float t = Mathf.InverseLerp(inMin, inMax, v);
        return Mathf.Lerp(outMin, outMax, t);
    }

    private void UpdateLowPassFilter()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 15f)
            return;

        float passAmount = Remap(0f, maxDistance, 22000, minPassVolume, distance);
        lowPassFilter.cutoffFrequency = passAmount;
    }

    public void PlayStep()
    {
        UpdateLowPassFilter();
        audioSource.pitch = 0.6f;
        audioSource.clip = step;
        audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        UpdateLowPassFilter();
        audioSource.pitch = 1f;
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    public void PlayVoiceClip(AudioClip clip)
    {
        UpdateLowPassFilter();
        killerVoice.pitch = 1f;
        killerVoice.clip = clip;
        killerVoice.Play();
    }

    public void PlayAngryPig()
    {
        UpdateLowPassFilter();
        killerVoice.pitch = 1f;
        killerVoice.clip = angryPig;
        killerVoice.Play();
    }

    private void UpdateAudioVolume()
    {
        float volume = PlayerPrefs.GetFloat("main_volume");
        killerVoice.volume = defaultVoiceVolume * volume;
        audioSource.volume = defaultVolume * volume;
    }
}
