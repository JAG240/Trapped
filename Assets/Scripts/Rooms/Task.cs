using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] private float cooldownTimer = 30f;
    public float taskTime = 5f;
    [SerializeField] protected float taskPosOffset = 2f;
    public bool onCooldown = false;
    [SerializeField] private bool RegisterTask = true;

    protected virtual void Start()
    {
        if(RegisterTask)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Room"));

            if (hits.Length > 0)
                hits[0].transform.GetComponent<Room>().AddRoomItem<Task>(this);
        }
    }

    public void CompleteTask()
    {
        onCooldown = true;
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        onCooldown = false;
    }

    public void CancelTask()
    {
        StopAllCoroutines();
        onCooldown = false;
    }

    public Vector3 GetTaskPosition()
    {
        return transform.position + (transform.forward * taskPosOffset);
    }

#if UNITY_EDITOR
    public bool showTaskPosition;

    private void OnDrawGizmos()
    { 
        if(showTaskPosition)
            Gizmos.DrawSphere(GetTaskPosition(), 0.2f);
    }

#endif
}
