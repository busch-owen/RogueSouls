using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [field: SerializeField]
    public List<GameObject> Keys { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> InventoryObjects { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> BossKeys { get; private set; } = new List<GameObject>();

    public void AddItemsToInventoryList(GameObject itemToAdd)
    {
        InventoryObjects.Add(itemToAdd);
        //Instantiate(itemToAdd);
    }

    public void AddKeysToKeysCount(GameObject keyToAdd)
    {
        Keys.Add(keyToAdd);
    }

    public void AddBossKeysToBossKeysCount(GameObject keyToAdd)
    {
        BossKeys.Add(keyToAdd);
    }
}