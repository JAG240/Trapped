using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTable : Task
{
    public PlaceableArea placeableArea { get; private set; }

    protected override void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Room"));

        if (hits.Length > 0)
            hits[0].transform.GetComponent<Room>().AddRoomItem<ChopTable>(this);

        placeableArea = GetComponent<PlaceableArea>();
    }

    public bool HasItem()
    {
        return placeableArea.needsChopped;
    }

    public override void CompleteTask()
    {
        placeableArea.CompleteChop();
    }
}
