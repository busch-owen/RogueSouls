using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    GameObject _itemToGivePlayer;

    Inventory _playerInventory;

    Animator _animator;

    bool _opened;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<Inventory>();
        _animator = GetComponentInChildren<Animator>();
        _opened = false;
    }

    public void OpenChest()
    {
        if(!_opened)
        {
            if (_itemToGivePlayer.GetComponent<Key>())
            {
                _playerInventory.AddKeysToKeysCount(_itemToGivePlayer);
            }
            else if(_itemToGivePlayer.GetComponent<BossKey>())
            {
                _playerInventory.AddBossKeysToBossKeysCount(_itemToGivePlayer);
            }
            else if (_itemToGivePlayer.GetComponent<Enemy>())
            {
                Instantiate(_itemToGivePlayer, this.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                _playerInventory.AddItemsToInventoryList(_itemToGivePlayer);
            }


            _animator.SetBool("ChestOpen", true);
            _opened = true;
        }
    }
}