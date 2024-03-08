using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    GameObject _itemToGivePlayer;

    Inventory _playerInventory;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<Inventory>();
    }

    public void OpenChest()
    {
        if(_itemToGivePlayer.GetComponent<Key>())
        {
            _playerInventory.AddKeysToKeysCount(_itemToGivePlayer);
        }
        else
        {
            _playerInventory.AddItemsToInventoryList(_itemToGivePlayer);
        }
    }
}