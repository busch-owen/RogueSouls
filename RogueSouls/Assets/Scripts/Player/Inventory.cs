using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _keys = new List<GameObject>();
    [SerializeField]
    List<GameObject> _inventoryObjects = new List<GameObject>();

    public void AddItemsToInventoryList(GameObject itemToAdd)
    {
        _inventoryObjects.Add(itemToAdd);
        Instantiate(itemToAdd);
    }

    public void AddKeysToKeysCount(GameObject keyToAdd)
    {
        _keys.Add(keyToAdd);
    }
}