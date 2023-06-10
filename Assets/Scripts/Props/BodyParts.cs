using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour, IInteractable
{
    private IChopInteraction[] chopReactions = new IChopInteraction[0];

    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerInventory>().PutInHands(gameObject, true);
        chopReactions = GetComponents<IChopInteraction>();
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
