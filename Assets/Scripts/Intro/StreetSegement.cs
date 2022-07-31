using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSegement : MonoBehaviour
{
    [field: SerializeField] public StreetBuilder streetBuilder { get; set; }

    void Update()
    {
        if (transform.position.x + transform.localScale.x <= streetBuilder.streetEnd)
        {
            transform.position = streetBuilder.GetResetPos(transform);
            return;
        }

        transform.position += streetBuilder.speed * Time.deltaTime;
    }
}
