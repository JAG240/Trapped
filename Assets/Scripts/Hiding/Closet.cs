using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Closet : Hiding, IInteractable, IPeekable
{
    private GameObject[] doors = new GameObject[2];
    private bool open = false;
    private Hiding hiding;
    [SerializeField] private float peekAngle = 5f;
    [SerializeField] private Vector3 characterOffset;
    [SerializeField] private bool Register = true;

    [Header("Sounds")]
    [SerializeField] private AudioClip slamOpen;
    [SerializeField] private AudioClip close;
    [SerializeField] private AudioClip playerInteract;
    private AudioSource audioSource;

    private void Start()
    {
        if(Register)
            AddToRoom();

        doors[0] = transform.Find("Door_L").gameObject;
        doors[1] = transform.Find("Door_R").gameObject;
        hiding = GetComponent<Hiding>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact(GameObject player)
    {
        StarterAssetsInputs inputs = player.GetComponentInChildren<StarterAssetsInputs>();
        FirstPersonController inputController = player.GetComponentInChildren<FirstPersonController>();
        CharacterController charController = player.GetComponentInChildren<CharacterController>();
        Transform playerCapsule = inputs.transform;

        if (!open)
        {
            open = true;

            charController.enabled = false;
            inputs.movementDisabled = true;
            inputs.cameraMovementDisabled = true;
            inputs.StopMovement();
            playerCapsule.transform.position = transform.position + characterOffset;

            audioSource.clip = playerInteract;
            audioSource.Play();

            inputController.LookAt(GetPosition() + characterOffset);
            hiding.IncreaseCounter();
            return;
        }

        open = false;

        charController.enabled = false;
        playerCapsule.transform.position = transform.position + characterOffset + (-transform.right * 2);

        audioSource.Play();

        inputs.movementDisabled = false;
        inputs.cameraMovementDisabled = false;
        charController.enabled = true;
    }

    public void OpenDoors(bool state)
    {
        if(state)
        {
            //open doors
            doors[0].transform.RotateAround(doors[0].transform.position, Vector3.up, 90);
            doors[1].transform.RotateAround(doors[1].transform.position, Vector3.up, -90);

            audioSource.clip = slamOpen;
            audioSource.Play();
        }
        else
        {
            //close doors
            doors[0].transform.RotateAround(doors[0].transform.position, Vector3.up, -90);
            doors[1].transform.RotateAround(doors[1].transform.position, Vector3.up, 90);

            audioSource.clip = close;
            audioSource.Play();
        }
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
