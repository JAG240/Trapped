using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLantern : Lantern, IInteractable
{
    public new void Interact(GameObject player)
    {
        audioSource.Play();
    }

    public void KillerInteract()
    {
        lit = !lit;

        currentMaxIntensity = lit ? maxIntensity : 0;
        currentMinIntensity = lit ? minIntensity : 0;
        ChangeLanternMat();
        audioSource.Play();
    }
}
