using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string keyID { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private Material material;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material renderOnTop;

    private void Start()
    {
        if (keyID == "")
            Debug.LogError($"{name} does not have a keyID!");

        audioSource = GetComponent<AudioSource>();
        material = GetComponent<MeshRenderer>().material;
    }

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory.HandsFull(false))
        {
            inventory.DropItem(false);
        }

        inventory.PutInHands(gameObject, false);
        audioSource.Play();
    }
}
