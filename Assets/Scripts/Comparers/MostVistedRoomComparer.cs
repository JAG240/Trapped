using System.Collections.Generic;

public class MostVistedRoomComparer : IComparer<Room>
{
    public int Compare(Room x, Room y)
    {
        return y.visitCount.CompareTo(x.visitCount);
    }
}
