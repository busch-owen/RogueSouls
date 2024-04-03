using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    WeaponWheel _playerWeaponWheel;

    [SerializeField]
    GameObject _slotPrompt;

    int _itemInCurrentSlot = 0;

    bool _awaitingBindResponse;

    private void Awake()
    {
        _playerWeaponWheel = FindObjectOfType<WeaponWheel>();
        
    }

    private void OnEnable()
    {
        _slotPrompt.SetActive(false);
        _awaitingBindResponse = false;
    }

    public void RequestBindSlot(Vector2 whichSlot)
    {
        if (_awaitingBindResponse)
        {
            float slotAngle = Mathf.Atan2(whichSlot.y, whichSlot.x) * Mathf.Rad2Deg;

            switch (slotAngle)
            {
                case 0:
                    _playerWeaponWheel.PutItemInDesiredSlot(0, _itemInCurrentSlot);
                    _slotPrompt.SetActive(false);
                    _awaitingBindResponse = false;
                    break;
                case -90:
                    _playerWeaponWheel.PutItemInDesiredSlot(1, _itemInCurrentSlot);
                    _slotPrompt.SetActive(false);
                    _awaitingBindResponse = false;
                    break;
                case 180:
                    _playerWeaponWheel.PutItemInDesiredSlot(2, _itemInCurrentSlot);
                    _slotPrompt.SetActive(false);
                    _awaitingBindResponse = false;
                    break;
                case 90:
                    _playerWeaponWheel.PutItemInDesiredSlot(3, _itemInCurrentSlot);
                    _awaitingBindResponse = false;
                    _slotPrompt.SetActive(false);
                    break;
            }
        }
    }

    public void OpenPromtWindow(int itemInSlot)
    {
        _playerWeaponWheel.LoadItemsOntoPlayer();
        _slotPrompt.SetActive(true);
        _itemInCurrentSlot = itemInSlot;
        _awaitingBindResponse = true;
    }
}
