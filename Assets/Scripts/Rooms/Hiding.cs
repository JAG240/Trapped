using UnityEngine;

public class Hiding : MonoBehaviour
{
    public int HideCount { get; private set; }

    void Start()
    {
        HideCount = 0;
        transform.root.GetComponent<Room>().AddHiding(this);
    }

    public void IncreaseCounter()
    {
        HideCount++;
    }
}
