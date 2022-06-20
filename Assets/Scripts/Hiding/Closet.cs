using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour, IInteractable, IPeekable
{
    GameObject[] doors = new GameObject[2];
    private bool open = false;
    [SerializeField] float peekAngle = 5f;

    private void Start()
    {
        doors[0] = transform.Find("Door_L").gameObject;
        doors[1] = transform.Find("Door_R").gameObject;
    }

    public void Interact()
    {
        if(!open)
        {
            open = true;
            doors[0].transform.RotateAround(doors[0].transform.position, Vector3.up, 90f);
            doors[1].transform.RotateAround(doors[1].transform.position, Vector3.up, -90f);
            return;
        }

        open = false;
        doors[0].transform.RotateAround(doors[0].transform.position, Vector3.up, -90f);
        doors[1].transform.RotateAround(doors[1].transform.position, Vector3.up, 90f);
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
}
