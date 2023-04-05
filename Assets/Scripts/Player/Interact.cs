using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private IPeekable objPeek;
    private GameObject peekedObj;
    private LayerMask interactMask;
    private LayerMask peekMask;
    private PlayerInventory playerInventory;

    private void Start()
    {
        interactMask = LayerMask.GetMask("Interactable", "Bottle");
        peekMask = LayerMask.GetMask("Peekable");
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnInteract()
    {
        Activate();
    }

    private void OnPeek(InputValue value)
    {
        bool state = value.Get<float>() == 1;

        if (state)
            Peek();
        else
            Unpeek();
    }

    private void Activate()
    {
        if (playerInventory.HandsFull())
            playerInventory.DropItem();

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, interactMask);

        if (hits.Length > 0)
        {
            GameObject obj = hits[0].transform.gameObject;
            IInteractable objInteract = obj.GetComponent<IInteractable>();

            if (objInteract != null)
            {
                objInteract.Interact(gameObject);
            }
        }
    }

    private void Peek()
    {
        if (playerInventory.HandsFull())
            playerInventory.ThrowItem();

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, peekMask);

        if (hits.Length > 0)
        {
            peekedObj = hits[0].transform.gameObject;
            objPeek = peekedObj.transform.parent.GetComponent<IPeekable>();

            if (objPeek != null)
            {
                objPeek.Peek(peekedObj.transform.gameObject, true);
            }
        }
    }

    private void Unpeek()
    {
        if (objPeek == null || peekedObj == null)
            return;

        objPeek.Peek(peekedObj, false);
        objPeek = null;
        peekedObj = null;
    }


}
