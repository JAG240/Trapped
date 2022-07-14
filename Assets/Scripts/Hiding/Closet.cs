using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : Hiding, IInteractable, IPeekable
{
    private GameObject[] doors = new GameObject[2];
    private bool open = false;
    private Hiding hiding;
    [SerializeField] private float peekAngle = 5f;
    [SerializeField] private Vector3 characterOffset;

    private void Start()
    {
        AddToRoom();
        doors[0] = transform.Find("Door_L").gameObject;
        doors[1] = transform.Find("Door_R").gameObject;
        hiding = GetComponent<Hiding>();
    }

    public void Interact(GameObject player)
    {
        if(!open)
        {
            open = true;
            player.GetComponentInChildren<CameraController>().EnableCameraMovement(false);
            player.transform.position = transform.position + characterOffset;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.transform.LookAt(GetPosition() + characterOffset + (transform.up * 0.2f), Vector3.up);
            hiding.IncreaseCounter();
            return;
        }

        open = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = transform.position + characterOffset + (-transform.right * 2);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponentInChildren<CameraController>().EnableCameraMovement(true);
    }

    public void Peek(GameObject obj, bool state)
    {
        GameObject door = obj.name == "Door_L" ? doors[0] : doors[1];
        float doorNormalize = obj.name == "Door_L" ? 1 : -1;

        if(state)
            door.transform.RotateAround(door.transform.position, Vector3.up, peekAngle * doorNormalize);
        else
            door.transform.RotateAround(door.transform.position, Vector3.up, -peekAngle * doorNormalize);
    }

    public override Vector3 GetPosition()
    {
        return transform.position + (-transform.right * 2);
    }
}
