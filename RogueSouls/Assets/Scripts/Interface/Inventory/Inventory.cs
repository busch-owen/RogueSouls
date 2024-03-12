using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [field: SerializeField]
    public List<GameObject> _keys { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> _inventoryObjects { get; private set; } = new List<GameObject>();

    public void AddItemsToInventoryList(GameObject itemToAdd)
    {
        _inventoryObjects.Add(itemToAdd);
        //Instantiate(itemToAdd);
    }

    public void AddKeysToKeysCount(GameObject keyToAdd)
    {
        _keys.Add(keyToAdd);
    }
}