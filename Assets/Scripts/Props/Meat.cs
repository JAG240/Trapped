using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour, IInteractable
{
    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerInventory>().PutInHands(gameObject, true);
    }
}
