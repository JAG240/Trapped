using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] private float cooldownTimer = 30;
    public bool onCooldown = false;

    void Start()
    {
        transform.root.GetComponent<Room>().AddTask(this);
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
}
