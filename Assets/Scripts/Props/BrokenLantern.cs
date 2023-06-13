using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLantern : Lantern, IInteractable
{
    [SerializeField] private AudioClip failedAudio;
    [SerializeField] private AudioClip successAudio;

    public new void Interact(GameObject player)
    {
        audioSource.clip = failedAudio;
        audioSource.Play();
    }

    public void KillerInteract()
    {
        lit = !lit;

        currentMaxIntensity = lit ? maxIntensity : 0;
        currentMinIntensity = lit ? minIntensity : 0;
        ChangeLanternMat();
        audioSource.clip = successAudio;
        audioSource.Play();
    }
}
