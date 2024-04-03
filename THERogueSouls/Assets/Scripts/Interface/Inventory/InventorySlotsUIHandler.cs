using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsUIHandler : MonoBehaviour
{
    [SerializeField]
    Image[] _slotItemImages;

    Inventory _inventory;

    private void Awake()
    {
        _inventory = FindObjectOfType<Inventory>();
        _slotItemImages = GetComponentsInChildren<Image>();

        for (int i = 0; i < _slotItemImages.Length; i++)
        {
            _slotItemImages[i].enabled = false;
        }
    }

    private void OnEnable()
    {
        UpdateSlotSprites();
    }

    public void UpdateSlotSprites()
    {
        for(int i = 0; i < _inventory.InventoryObjects.Count; i++)
        {
            _slotItemImages[i].enabled = true;
            _slotItemImages[i].sprite = _inventory.InventoryObjects[i].GetComponentInChildren<SpriteRenderer>().sprite;
            _slotItemImages[i].SetNativeSize();
        }
    }
}
