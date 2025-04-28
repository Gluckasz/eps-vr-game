using System.Collections.Generic;
using UnityEngine;

public class ItemDiscoveryManager : MonoBehaviour
{
    public static ItemDiscoveryManager instance;

    private HashSet<string> discoveredItems = new HashSet<string>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void DiscoverItem(string itemName)
    {
        if (!discoveredItems.Contains(itemName))
        {
            discoveredItems.Add(itemName);
            Debug.Log("Discovered new item: " + itemName);
        }
    }

    public bool HasDiscovered(string itemName)
    {
        return discoveredItems.Contains(itemName);
    }
}
