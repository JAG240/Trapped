using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour, IInteractable
{
    private IChopInteraction[] chopReactions = new IChopInteraction[0];

    private void Start()
    {
        chopReactions = GetComponents<IChopInteraction>();
    }

    public void Interact(GameObject player)
    {
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();

        if (playerInventory.HandsFull(true))
            return;

        playerInventory.PutInHands(gameObject, true);
    }

    public void GetChopped(PlaceableArea area)
    {
        if(chopReactions.Length > 0)
        {
            foreach(IChopInteraction reaction in chopReactions)
            {
                reaction.ChopInteract(area);
            }
        }
    }
}
