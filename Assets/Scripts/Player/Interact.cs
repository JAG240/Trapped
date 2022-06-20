using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private PlayerControls playerControls;
    private IPeekable objPeek;
    private GameObject peekedObj;

    private void Start()
    {
        playerControls = GameObject.Find("InputManager").GetComponent<InputManager>().playerControls;
        playerControls.Basic.Interact.performed += (context) => Activate();
        playerControls.Basic.Peek.performed += (context) => Peek();
        playerControls.Basic.Peek.canceled += (context) => Unpeek();
    }

    private void Activate()
    {
        RaycastHit obj;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out obj, 3, LayerMask.GetMask("Interactable")))
        {
            IInteractable objInteract = obj.transform.GetComponent<IInteractable>();

            if (objInteract != null)
            {
                objInteract.Interact();
            }
        }
    }

    private void Peek()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 3, LayerMask.GetMask("Peekable"));

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
