using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KillerAudio : MonoBehaviour
{
    [SerializeField] float maxDistance = 25f;
    [SerializeField] float minPassVolume = 230f;
    [SerializeField] private AudioClip step;
    [SerializeField] private AudioClip effortGrunt;
    [SerializeField] private AudioClip fullChop;
    private Transform player;
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        
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
}
