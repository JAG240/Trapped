using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private Key key;
    private string keyID;

    private void Start()
    {
        keyID = key.keyID;
    }

    public bool Unlock(PlayerInventory playerInventory)
    {
        if(playerInventory.ConsumeKey(keyID))
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
