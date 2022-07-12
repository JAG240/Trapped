using UnityEngine;

public abstract class Hiding : MonoBehaviour
{
    public int HideCount { get; private set; } = 0;

    public void IncreaseCounter()
    {
        HideCount++;
    }

    protected void AddToRoom()
    {
        transform.root.GetComponent<Room>().AddRoomItem<Hiding>(this);
    }

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }
}
