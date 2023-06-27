using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string keyID { get; private set; }
    private AudioSource audioSource;
    private SceneManager sceneManager;
    private float defaultVolume;

    private void Start()
    {
        if (keyID == "")
            Debug.LogError($"{name} does not have a keyID!");

        audioSource = GetComponent<AudioSource>();

        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.playerPrefsUpdated += UpdateVolume;
        defaultVolume = audioSource.volume;
    }

    private void OnDestroy()
    {
        if (sceneManager == null)
            return;

        sceneManager.playerPrefsUpdated -= UpdateVolume;
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

    private void UpdateVolume()
    {
        float volume = PlayerPrefs.GetFloat("main_volume");
        audioSource.volume = defaultVolume * volume;
    }
}
