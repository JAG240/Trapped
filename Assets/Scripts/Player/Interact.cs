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
    private UIManager uiManager;
    private SceneManager sceneManager;

    private void Start()
    {
        interactMask = LayerMask.GetMask("Interactable", "Bottle");
        peekMask = LayerMask.GetMask("Peekable");
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.resetLevel += ResetLevel;
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, interactMask);

        if (hits.Length <= 0)
        {
            uiManager.SetCrossHairColor(false);
            return;
        }

        GameObject obj = hits[0].transform.gameObject;

        if (obj.GetComponent<PlaceableArea>() != null || obj.GetComponent<IInteractable>() != null)
            uiManager.SetCrossHairColor(true);
        else if (obj.transform.parent != null && obj.transform.parent.GetComponent<IInteractable>() != null)
            uiManager.SetCrossHairColor(true);
        else
            uiManager.SetCrossHairColor(false);
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
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, interactMask);

        if (hits.Length > 0)
        {
            GameObject obj = hits[0].transform.gameObject;

            PlaceableArea placeObj = obj.GetComponent<PlaceableArea>();

            if (placeObj != null && !playerInventory.HandsFull(true) && placeObj.placedObject != null)
            {
                placeObj.RemoveFromPlace(gameObject);
                return;
            }

            IInteractable objInteract = obj.GetComponent<IInteractable>();

            if (objInteract != null)
            {
                objInteract.Interact(gameObject);
                return;
            }

            if (obj.transform.parent == null)
                return;

            IInteractable parentInteract = obj.transform.parent.GetComponent<IInteractable>();

            if(parentInteract != null)
            {
                parentInteract.Interact(gameObject);
                return;
            }
        }

        if (playerInventory.HandsFull(true))
            playerInventory.DropItem(true);
        else if (playerInventory.HandsFull(false))
            playerInventory.DropItem(false);
    }

    private void Peek()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, peekMask);

        if (hits.Length > 0)
        {
            peekedObj = hits[0].transform.gameObject;
            objPeek = peekedObj.transform.parent.GetComponent<IPeekable>();

            if (objPeek != null)
            {
                objPeek.Peek(peekedObj.transform.gameObject, true);
                return;
            }
        }

        if (playerInventory.HandsFull(true))
            playerInventory.ThrowItem();
    }

    private void Unpeek()
    {
        if (objPeek == null || peekedObj == null)
            return;

        objPeek.Peek(peekedObj, false);
        objPeek = null;
        peekedObj = null;
    }

    private void ResetLevel()
    {
        playerInventory.ClearHands();
    }
}
