using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<string> keys = new List<string>();

    public bool ConsumeKey(string key)
    {
        if(keys.Contains(key))
        {
            keys.Remove(key);
            return true;
        }

        return false;
    }

    public void AddKey(string key)
    {
        keys.Add(key);
    }
}
