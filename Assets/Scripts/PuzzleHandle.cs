using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandle : MonoBehaviour, IInteractable
{
    [SerializeField] private Lantern lantern;
    [SerializeField] private GameObject chainWrapping;
    [SerializeField] private GameObject extraChainLength;
    [field: SerializeField] public bool lowered { get; private set; }
    [SerializeField] private Transform lanternLoweredPos;
    [SerializeField] private Transform lanternRaisedPos;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateChains(lowered);
    }

    public void Interact(GameObject player)
    {
        lowered = !lowered;
        UpdateChains(lowered);
        audioSource.Play();
    }

    private void UpdateChains(bool state)
    {
        chainWrapping.SetActive(!state);
        extraChainLength.SetActive(state);
        lantern.UpdateInteractable(lowered);
        lantern.transform.position = state ? lanternLoweredPos.position : lanternRaisedPos.position;
    }
}
