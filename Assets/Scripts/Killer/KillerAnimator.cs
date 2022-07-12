using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Movement", speed);
    }

    public void SetChop(bool state)
    {
        animator.SetBool("Chop", state);
    }
}
