using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbRemovalChop : MonoBehaviour, IChopInteraction
{
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject body;


    void IChopInteraction.ChopInteract(PlaceableArea area)
    {
        //Seperate limbs
        area.ClearSpace();

        arm.transform.parent = null;
        body.transform.parent = null;

        arm.GetComponent<Collider>().enabled = true;
        body.GetComponent<Collider>().enabled = true;

        arm.GetComponent<Rigidbody>().isKinematic = false;
        body.GetComponent<Rigidbody>().isKinematic = false;

        Destroy(gameObject);
    }
}
