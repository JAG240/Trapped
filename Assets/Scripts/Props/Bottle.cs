using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour, IStateComparable, IInteractable
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float susDistance = 1f;
    [SerializeField] private AudioClip smallDrop;
    [SerializeField] private AudioClip bigDrop;
    [SerializeField] private float audioForce = 2f;
    [SerializeField] private float audioForceBig = 4f;
    private GhostBottle ghost;
    private AudioSource audioSource;
    private RoomManager roomManager;

    private void Start()
    {
        GameObject objGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost = objGhost.GetComponent<GhostBottle>();
        ghost.SetParent(this);
        ghost.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    public bool StateChanged()
    {
        if(ghost.gameObject.activeInHierarchy)
        {
            bool isSus = CheckGhostDistance();
            ResetGhostPosition();
            return isSus;
        }
        else
        {
            ResetGhostPosition();
            return false;
        }
    }

    public bool CheckGhostDistance()
    {
        float distance = Vector3.Distance(transform.position, ghost.transform.position);

        if(distance != 0)
        {
            ghost.gameObject.SetActive(false);
            return distance > susDistance;
        }

        ghost.RefreshMemory();
        return false;
    }

    private void ResetGhostPosition()
    {
        ghost.gameObject.SetActive(true);
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource.isPlaying)
            return;

        if(collision.relativeVelocity.magnitude > audioForceBig)
        {
            audioSource.clip = bigDrop;
            audioSource.Play();
            roomManager.KillerInRoomAudio(gameObject);
            roomManager.KillerInConnectedRoomAudio(gameObject);
        }
        else if(collision.relativeVelocity.magnitude > audioForce)
        {
            audioSource.clip = smallDrop;
            audioSource.Play();
            roomManager.KillerInRoomAudio(gameObject);
        }
    }

    public void Interact(GameObject player)
    {
        player.GetComponent<PlayerInventory>().PutInHands(gameObject);
    }
}
