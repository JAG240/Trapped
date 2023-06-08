using UnityEngine;

public abstract class Hiding : MonoBehaviour
{
    [SerializeField] private Vector3 checkOffset;
    [SerializeField] private bool showCheckOffset;
    public int HideCount { get; private set; } = 0;

    public void IncreaseCounter()
    {
        HideCount++;
    }

    protected void AddToRoom()
    {
        transform.parent.GetComponentInChildren<Room>().AddRoomItem<Hiding>(this);
    }

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetCheckPosition()
    {
        return transform.position + checkOffset;
    }

    private void OnDrawGizmos()
    {
        if (showCheckOffset)
            Gizmos.DrawSphere(GetCheckPosition(), 0.2f);
    }
}
