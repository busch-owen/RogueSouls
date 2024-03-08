using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    GameObject _itemToGivePlayer;

    Inventory _playerInventory;

    Animator _animator;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<Inventory>();
        _animator = GetComponentInChildren<Animator>();
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

        _animator.SetBool("ChestOpen", true);
    }
}