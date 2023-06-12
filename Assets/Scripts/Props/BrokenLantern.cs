using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLantern : Lantern, IInteractable
{
    public new void Interact(GameObject player)
    {
        audioSource.Play();
    }
}
