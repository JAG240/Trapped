using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float detectionTimer = 0.2f;
    [SerializeField] int memorySize = 10;
    [SerializeField] float detectionAngle = 90;
    [SerializeField] LayerMask detectionMasks;
    [SerializeField] Transform eyes;

    private void Start()
    {
        StartCoroutine(CheckVision());
    }

    //Debug Method to see detection sphere
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/

    private IEnumerator CheckVision()
    {
        Collider[] results = new Collider[memorySize];
        Vector3 direction;
        RaycastHit hit;
        float angle;

        while(true)
        {
            if(Physics.OverlapSphereNonAlloc(transform.position, radius, results, detectionMasks) >  0)
            {
                foreach (Collider obj in results)
                {
                    if (!obj)
                        continue;

                    direction = obj.transform.position - eyes.transform.position;
                    angle = Mathf.Abs(Vector3.Angle(transform.forward, direction));

                    if (angle > detectionAngle || angle < -detectionAngle)
                        continue;

                        //Debug.DrawRay(obj.transform.position, -direction, Color.yellow, 5f);

                    if (Physics.Raycast(obj.transform.position, -direction, out hit, radius))
                    {
                        if (hit.transform.tag != "Killer")
                            continue;

                        IStateComparable objState;

                        if(obj.transform.parent != null)
                            objState = obj.transform.parent.GetComponent<IStateComparable>();
                        else
                            objState = obj.transform.GetComponent<IStateComparable>();

                        if (objState != null && objState.StateChanged())
                            Debug.Log($"{obj.name} has changed state");
                    }
                }
            }

            yield return new WaitForSeconds(detectionTimer);
        }
    }
}