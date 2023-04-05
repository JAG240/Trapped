using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string keyID { get; private set; }
    private AudioSource audioSource;
    private MeshRenderer mesh;
    private CapsuleCollider capCollider;

    private void Start()
    {
        if (keyID == "")
            Debug.LogError($"{name} does not have a keyID!");

        audioSource = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        capCollider = GetComponentInChildren<CapsuleCollider>();
    }

    public void Interact(GameObject player)
    {
        mesh.enabled = false;
        capCollider.enabled = false;
        player.GetComponent<PlayerInventory>().AddKey(keyID);
        StartCoroutine(Pickup());
    }

    private IEnumerator Pickup()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
