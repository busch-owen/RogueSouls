using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    WeaponWheel _playerWeaponWheel;

    private void Awake()
    {
        _playerWeaponWheel = FindObjectOfType<WeaponWheel>();
    }

    public void EquipToWheelSlot(int itemIndex)
    {
        //_playerWeaponWheel.PutItemInDesiredSlot(itemIndex, _playerWeaponWheel.equippedItems[itemIndex]);
    }
}
