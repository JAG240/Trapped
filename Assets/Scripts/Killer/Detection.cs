using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Detection : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float detectionTimer = 0.2f;
    [SerializeField] int memorySize = 10;
    [SerializeField] float detectionAngle = 90;
    [SerializeField] LayerMask detectionMasks;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] Transform eyes;
    public event Action<GameObject> detectedObject;
    [SerializeField] private bool playerInSight = false;
    [SerializeField] private LayerMask checkMask;
    private SceneManager sceneManager;

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        StartCoroutine(CheckVision());
        checkMask = detectionMasks;

        sceneManager.resetLevel += ResetSeen;
    }

    //Debug Method to see detection sphere
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/

    private void ResetSeen()
    {
        SetPlayerInSight(null);
    }

    private IEnumerator CheckVision()
    {
        Collider[] results = new Collider[memorySize];
        Vector3 direction;
        RaycastHit hit;
        float angle;

        while(true)
        {
            if(Physics.OverlapSphereNonAlloc(transform.position, radius, results, checkMask) >  0)
            {
                foreach (Collider obj in results)
                {
                    if (!obj)
                        continue;

                    direction = obj.transform.position - eyes.transform.position;
                    angle = Mathf.Abs(Vector3.Angle(transform.forward, direction));

                    if (angle > detectionAngle || angle < -detectionAngle)
                    {
                        if (playerInSight && obj.name == "PlayerCapsule")
                            SetPlayerInSight(null);

                        continue;
                    }

                    //Debug.DrawRay(obj.transform.position, -direction, Color.yellow, 5f);

                    if (Physics.Raycast(obj.transform.position, -direction, out hit, radius, raycastLayerMask))
                    {
                        if (hit.transform.tag != "Killer")
                        {
                            if(obj.name == "PlayerCapsule" && playerInSight)
                                SetPlayerInSight(null);

                            continue;
                        }

                        if(obj.name == "PlayerCapsule")
                        {
                            if (playerInSight)
                                continue;

                            SetPlayerInSight(obj.gameObject);
                            continue;
                        }

                        IStateComparable objState;

                        if(obj.transform.parent != null)
                            objState = obj.transform.parent.GetComponent<IStateComparable>();
                        else
                            objState = obj.transform.GetComponent<IStateComparable>();

                        if (objState != null && objState.StateChanged())
                            detectedObject.Invoke(obj.gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(detectionTimer);
        }
    }

    private void SetPlayerInSight(GameObject obj)
    {
        if(obj)
        {
            playerInSight = true;
            detectedObject.Invoke(obj);
            checkMask = LayerMask.GetMask("Player");
        }
        else
        {
            playerInSight = false;
            detectedObject.Invoke(null);
            checkMask = detectionMasks;
        }
    }
}