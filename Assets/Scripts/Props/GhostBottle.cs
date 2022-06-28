using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBottle : MonoBehaviour, IStateComparable
{
    [SerializeField] private float ghostMemory = 10f;
    private Bottle parent;

    public bool StateChanged()
    {
        return parent.CheckGhostDistance();
    }

    private void OnEnable()
    {
        StartCoroutine(StartGhostMemory());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartGhostMemory()
    {
        yield return new WaitForSeconds(ghostMemory);
        gameObject.SetActive(false);
    }

    public void RefreshMemory()
    {
        StopAllCoroutines();
        StartCoroutine(StartGhostMemory());
    }

    public void SetParent(Bottle bottle)
    {
        parent = bottle;
    }
}
