using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInventory : MonoBehaviour
{
    public static event Action<KeyType> OnKeyCollected;

    private HashSet<KeyType> keys = new HashSet<KeyType>();

    public void AddKey(KeyType key)
    {
        if (keys.Add(key))
        {
            Debug.Log("Picked up key: " + key);
            OnKeyCollected?.Invoke(key);
        }
    }

    public bool HasKey(KeyType key)
    {
        return keys.Contains(key);
    }

    public void ClearKeys()
    {
        keys.Clear();
    }
}
