using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string keyID { get; private set; }

    private void Start()
    {
        if (keyID == "")
            Debug.LogError($"{name} does not have a keyID!");
    }

    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerInventory>().AddKey(keyID);
        Destroy(gameObject);
    }
}
