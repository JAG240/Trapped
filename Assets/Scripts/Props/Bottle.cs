using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour, IStateComparable
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float susDistance = 1f;
    private GhostBottle ghost;

    private void Start()
    {
        GameObject objGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost = objGhost.GetComponent<GhostBottle>();
        ghost.SetParent(this);
        ghost.gameObject.SetActive(false);
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
}
