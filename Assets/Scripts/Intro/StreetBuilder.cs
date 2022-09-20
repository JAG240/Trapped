using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class StreetBuilder : MonoBehaviour
{
    [SerializeField] private GameObject streetSegment;
    [field: SerializeField] public float streetStart { get; private set; } = 0f;
    [field: SerializeField] public float streetEnd { get; private set; } = 0f;
    [field: SerializeField] public Vector3 speed { get; private set; } = new Vector3(-0.3f, 0f, 0f);
    [SerializeField] private Transform mostRecent;
    [SerializeField] private float stopTimer = 3.0f;

    public Vector3 GetResetPos(Transform currentSegement)
    {
        float x = mostRecent.position.x + (currentSegement.transform.localScale.x * 2); // + (speed.x * Time.deltaTime);
        mostRecent = currentSegement;
        return new Vector3(x, transform.position.y, transform.position.z);
    }

    public IEnumerator StopMovement()
    {
        float totalTime = 0f;
        float startSpeed = speed.x;
        while(totalTime < stopTimer)
        {
            float t = totalTime / stopTimer;
            float newSpeed = Mathf.Lerp(startSpeed, 0f, t);
            speed = new Vector3(newSpeed, 0f, 0f);
            totalTime += Time.deltaTime;
            yield return null;
        }

        speed = Vector3.zero;
    }

#if UNITY_EDITOR
    public bool showStreetEnds = false;
    [ReadOnly] public int calcedSegments; 

    private void OnDrawGizmos()
    {
        calcedSegments = Mathf.CeilToInt((streetStart - streetEnd) / (streetSegment.transform.localScale.x * 2));

        if (!showStreetEnds)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(streetStart, transform.position.y, transform.position.z), 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(streetEnd, transform.position.y, transform.position.z), 1f);
    }

    public void BuildStreet()
    {
        int neededSegements = Mathf.CeilToInt((streetStart - streetEnd) / (streetSegment.transform.localScale.x * 2));

        for (int i = 0; i < neededSegements; i++)
        {
            float xPos = (streetStart - streetSegment.transform.localScale.x) - ((streetSegment.transform.localScale.x * 2) * i);
            GameObject street = Instantiate(streetSegment, new Vector3(xPos, transform.position.y, transform.position.z), Quaternion.identity);
            street.GetComponent<StreetSegement>().streetBuilder = this;

            if (i == 0)
                mostRecent = street.transform;
        }
    }
#endif
}
