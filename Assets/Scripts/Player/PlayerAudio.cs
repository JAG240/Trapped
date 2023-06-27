using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private AudioSource soundFXAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip introMusicClip;
    [SerializeField] private AudioClip introSigh;

    [Header("Footstep Settings")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingCooldown;
    [SerializeField] private float runningCooldown;

    [Header("Concert")]
    [SerializeField] private List<AudioClip> dirtyGroundFootsteps = new List<AudioClip>();
    [SerializeField] private List<AudioClip> dirtyGroundFootstepsRunning = new List<AudioClip>();

    [Header("Wood")]
    [SerializeField] private List<AudioClip> woodenGroundFootsteps = new List<AudioClip>();
    [SerializeField] private List<AudioClip> woodenGroundFootstepsRunning = new List<AudioClip>();

    private CharacterController characterController;
    private float lastAudioClip = 0;
    public footstepSource source = footstepSource.concrete;
    private SceneManager sceneManager;
    private float defaultVoiceVolume;
    private float defaultFXVolume;

    [Serializable]
    public enum footstepSource
    {
        concrete, wood
    }

    void Start()
    {
        characterController = GetComponentInChildren<CharacterController>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.playerPrefsUpdated += UpdateVolume;
        sceneManager.playerEneteredPorch += EnterPorch;
        defaultFXVolume = soundFXAudioSource.volume;
        defaultVoiceVolume = voiceAudioSource.volume;
    }

    private void FixedUpdate()
    {
        lastAudioClip += Time.deltaTime;

        if (characterController.velocity.magnitude > runningSpeed && !soundFXAudioSource.isPlaying && lastAudioClip >= walkingCooldown)
        {
            List<AudioClip> audioSources = source == footstepSource.wood ? woodenGroundFootsteps : dirtyGroundFootsteps;

            lastAudioClip = 0f;
            soundFXAudioSource.pitch = 1.4f;
            soundFXAudioSource.clip = audioSources[UnityEngine.Random.Range(0, audioSources.Count)];
            soundFXAudioSource.Play();
        }
        else if(characterController.velocity.magnitude > walkingSpeed && !soundFXAudioSource.isPlaying && lastAudioClip >= runningCooldown)
        {
            List<AudioClip> audioSources = source == footstepSource.wood ? woodenGroundFootstepsRunning : dirtyGroundFootstepsRunning;

            lastAudioClip = 0f;
            soundFXAudioSource.pitch = 0.7f;
            soundFXAudioSource.clip = audioSources[UnityEngine.Random.Range(0, audioSources.Count)];
            soundFXAudioSource.Play();
        }
    }

    public void StartIntroMusic()
    {
        soundFXAudioSource.loop = true;
        soundFXAudioSource.clip = introMusicClip;
        soundFXAudioSource.Play();
    }

    public IEnumerator StopIntroMusic(float time)
    {
        float totalTime = 0f;
        float startMusicVolume = soundFXAudioSource.volume;

        while (totalTime < time)
        {
            float t = totalTime / time;
            soundFXAudioSource.volume = Mathf.Lerp(startMusicVolume, 0, t);
            totalTime += Time.deltaTime;
            yield return null;
        }

        soundFXAudioSource.Stop();
        soundFXAudioSource.loop = false;
        soundFXAudioSource.volume = 1f;
        PlaySigh();
    }

    private void PlaySigh()
    {
        voiceAudioSource.clip = introSigh;
        voiceAudioSource.Play();
    }

    private void UpdateVolume()
    {
        float volume = PlayerPrefs.GetFloat("main_volume");
        voiceAudioSource.volume = defaultVoiceVolume * volume;
        soundFXAudioSource.volume = defaultFXVolume * volume;
    }

    private void EnterPorch()
    {
        source = footstepSource.concrete;
    }
}
