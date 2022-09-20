using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSegement : MonoBehaviour
{
    [field: SerializeField] public StreetBuilder streetBuilder { get; set; }

    void FixedUpdate()
    {
        if (transform.position.x + transform.localScale.x <= streetBuilder.streetEnd)
        {
            transform.position = streetBuilder.GetResetPos(transform);
        }
    }

    private void LateUpdate()
    {
        transform.position += streetBuilder.speed * Time.deltaTime;
    }
}
