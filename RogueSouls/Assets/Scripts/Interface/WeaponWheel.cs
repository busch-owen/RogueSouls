using System.Collections;
using System.Collections.Generic;
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
    List<Transform> _itemSlots = new List<Transform>();

    [field: SerializeField]
    public List<GameObject> equippedItems { get; private set; } = new List<GameObject>();

    List<GameObject> _spawnedItems = new List<GameObject>(12);

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
        if(_spawnedItems.Count != 0)
        {
            LoadItemsOntoPlayer();
        }
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
        if(_spawnedItems.Count != 0)
        {
            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                _itemSlots[i].GetComponent<SpriteRenderer>().sprite = _spawnedItems[i].GetComponentInChildren<SpriteRenderer>().sprite;
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
        if(_spawnedItems.Count != 0)
        {
            foreach (GameObject item in _spawnedItems)
            {
                item.SetActive(false);
            }
        }
    }

    private void GrabWeaponsFromInventory()
    {
        foreach(GameObject items in _playerInventory.InventoryObjects)
        {
            equippedItems.Add(items);
        }
    }

    public void LoadItemsOntoPlayer()
    {
        GrabWeaponsFromInventory();
        Debug.Log("tried to load items onto player");
        foreach(GameObject objectToCheck in equippedItems)
        {
            //string itemName = objectToCheck.name;
            if (GameObject.Find("Player/Sprite/WeaponHandle/" + objectToCheck.name) == null)
            {
                Debug.Log("Looking for items to load onto player");
                GameObject spawnedItem = Instantiate(objectToCheck, _weaponOffsetHandle.transform);
                _spawnedItems.Add(spawnedItem);
            }
        }
    }

    public void PutItemInDesiredSlot(int desiredSlot, int itemToInsert)
    {
        PutAwayCurrentWeapon();
        //_spawnedItems.Add(_spawnedItems[itemToInsert]);
        //_spawnedItems.RemoveAt(desiredSlot);
        _spawnedItems.Insert(desiredSlot, equippedItems[itemToInsert]);
        EquipWeapon(desiredSlot);
    }

    public void EquipWeapon(int index)
    {
        if(index > _spawnedItems.Count - 1)
        {
            return;
        }
        _spawnedItems[index]?.SetActive(true);
        _weaponOffsetHandle.SetCurrentWeapon();
        _playerInputHandler.UpdateRangedWeaponReference();
    }
}
