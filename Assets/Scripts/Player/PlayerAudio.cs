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
    [SerializeField] private List<AudioClip> dirtyGroundFootsteps = new List<AudioClip>();
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponentInChildren<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (characterController.velocity.magnitude > runningSpeed && !soundFXAudioSource.isPlaying)
        {
            soundFXAudioSource.pitch = 1.4f;
            soundFXAudioSource.clip = dirtyGroundFootsteps[Random.Range(0, dirtyGroundFootsteps.Count)];
            soundFXAudioSource.Play();
        }
        else if(characterController.velocity.magnitude > walkingSpeed && !soundFXAudioSource.isPlaying)
        {
            soundFXAudioSource.pitch = 0.7f;
            soundFXAudioSource.clip = dirtyGroundFootsteps[Random.Range(0, dirtyGroundFootsteps.Count)];
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
}
