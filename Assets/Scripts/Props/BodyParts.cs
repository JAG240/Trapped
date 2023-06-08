using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour, IInteractable
{
    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerInventory>().PutInHands(gameObject, true);
    }

    public void GetChopped()
    {
        //Create chopped interaction Interface and make it so special scripts will trigger when chopped. 
        Debug.Log("I got chopped!");
    }
}
