using System.Collections.Generic;

public class MostUsedHidingComparer : IComparer<Hiding>
{
    public int Compare(Hiding x, Hiding y)
    {
        return y.HideCount.CompareTo(x.HideCount);
    }
}
