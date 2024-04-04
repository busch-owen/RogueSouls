using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponWheel : MonoBehaviour
{
    float _arrowAngle;

    [SerializeField]
    GameObject _weaponWheelObject;

    [SerializeField]
    Transform _arrow;

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    float _menuOpenTimeScale;

    Inventory _playerInventory;

    Camera _camera;

    Vector2 _aimPosition;

    [SerializeField]
    ItemHighlight _itemHighlight;

    [SerializeField]
    Transform[] _itemSlots = new Transform[4];

    [SerializeField]
    GameObject[] _itemsInInventory = new GameObject[12];

    [SerializeField]
    GameObject[] _itemsSpawnedOntoPlayer = new GameObject[12];

    [SerializeField]
    GameObject[] _itemsEquippedToWeaponWheel = new GameObject[4];

    PlayerController _playerController;

    WeaponOffsetHandle _weaponOffsetHandle;

    PlayerInputHandler _playerInputHandler;


    private void Awake()
    {
        _camera = Camera.main;
        _playerController = FindObjectOfType<PlayerController>();
        _weaponOffsetHandle = _playerController.GetComponentInChildren<WeaponOffsetHandle>();
        _playerInputHandler = _playerController.GetComponent<PlayerInputHandler>();
        _playerInventory = FindObjectOfType<Inventory>();
        _weaponWheelObject.SetActive(false);

        GrabWeaponsFromInventory();
        LoadItemsOntoPlayer();
    }

    private void Update()
    {
        HandleArrowRotation();
        HighlightSelectedWeapon();
    }

    private void HandleArrowRotation()
    {
        _arrowAngle = Mathf.Atan2(_aimPosition.y, _aimPosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_arrowAngle, Vector3.forward);
        _arrow.rotation = Quaternion.Slerp(_arrow.rotation, rotation, _rotateSpeed * Time.deltaTime);
    }

    public void HandleArrowInputMouse(Vector2 aimPosition)
    {
        if (_camera != null)
        {
            aimPosition = _camera.ScreenToWorldPoint(aimPosition) - _camera.transform.position;
            _aimPosition = aimPosition;
        }
    }
    public void HandleArrowInputController(Vector2 aimPosition)
    {
        _aimPosition = aimPosition;
    }

    public void OpenWeaponWheel()
    {
        if(_weaponWheelObject != null)
        {
            _weaponWheelObject.SetActive(true);
            FillItemSlots();
            Time.timeScale = _menuOpenTimeScale;
        }
    }

    public void CloseWeaponWheel()
    {
        if (_weaponWheelObject != null)
        {
            _weaponWheelObject.SetActive(false);
            PutAwayCurrentWeapon();
            EquipWeapon(WhichWeaponToSelect());

            Time.timeScale = 1;
        }
    }

    private void FillItemSlots()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemsEquippedToWeaponWheel[i] != null)
            {
                _itemSlots[i].GetComponent<SpriteRenderer>().sprite = _itemsEquippedToWeaponWheel[i].GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                _itemSlots[i].GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    private void HighlightSelectedWeapon()
    {
        _itemHighlight.transform.localPosition = _itemSlots[WhichWeaponToSelect()].transform.localPosition;
    }

    private int WhichWeaponToSelect()
    {
        if (_arrowAngle is < 135 and >= 45)
        {
            return 3;
        }
            
        else if (_arrowAngle is < 45 and >= -45)
        {
            return 0;
        }
        else if (_arrowAngle is < -45 and >= -135)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private void PutAwayCurrentWeapon()
    {
        foreach (GameObject item in _itemsSpawnedOntoPlayer)
        {
            if(item != null)
            {
                item.SetActive(false);
            }
            
        }
    }

    private void GrabWeaponsFromInventory()
    {
        for(int i = 0; i < _playerInventory.InventoryObjects.Count; i++)
        {
            _itemsInInventory[i] = _playerInventory.InventoryObjects[i];
        }
    }

    public void LoadItemsOntoPlayer()
    {
        GrabWeaponsFromInventory();
        for (int i = 0; i < _itemsInInventory.Length; i++)
        {
            if (_itemsInInventory[i] == null)
            {
                return;
            }
            string objectToCheck = _itemsInInventory[i].name;
            if (GameObject.Find("Player/Sprite/WeaponHandle/" + (_itemsInInventory[i]?.name + "(Clone)")) == null)
            {
                GameObject spawnedItem = Instantiate(_itemsInInventory[i], _weaponOffsetHandle.transform);
                if (spawnedItem != null)
                {
                    _itemsSpawnedOntoPlayer[i] = spawnedItem;
                }
            }
        }
    }

    public void PutItemInDesiredSlot(int desiredSlot, int itemToInsert)
    {
        PutAwayCurrentWeapon();

        _itemsEquippedToWeaponWheel[desiredSlot] = _itemsSpawnedOntoPlayer[itemToInsert];

        for (int i = 0; i < _itemsEquippedToWeaponWheel.Length; i++)
        {
            if (_itemsEquippedToWeaponWheel[i] != null && _itemsEquippedToWeaponWheel[desiredSlot] != null)
            {
                if (_itemsEquippedToWeaponWheel[i].name == _itemsEquippedToWeaponWheel[desiredSlot].name)
                {
                    Debug.Log(i);
                    _itemsEquippedToWeaponWheel[i] = null;
                }
            }
        }
        _itemsEquippedToWeaponWheel[desiredSlot] = _itemsSpawnedOntoPlayer[itemToInsert];
        EquipWeapon(desiredSlot);
    }

    public void EquipWeapon(int index)
    {
        if (_itemsEquippedToWeaponWheel[index] != null)
        {
            _itemsEquippedToWeaponWheel[index].SetActive(true);
            _weaponOffsetHandle.SetCurrentWeapon();
            _playerInputHandler.UpdateRangedWeaponReference();
        }
    }
}
