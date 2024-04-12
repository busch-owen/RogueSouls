using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    GameObject _itemToGivePlayer;

    UIHandler _handler;

    HUD _pickupDisplay;

    Inventory _playerInventory;

    Animator _animator;

    public bool Opened { get; private set; }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<Inventory>();
        _animator = GetComponentInChildren<Animator>();
        _pickupDisplay = FindAnyObjectByType<HUD>();
        _handler = FindObjectOfType<UIHandler>();
        Opened = false;
    }

    public void OpenChest()
    {
        if(!Opened)
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

            _pickupDisplay.ShowSpecificMessageOnTextBox("You got a " + _itemToGivePlayer.GetComponent<CollectableItem>().ItemName + "! \nItem was added to your inventory!", 2f);
            _animator.SetBool("ChestOpen", true);
            Opened = true;
        }
    }
}