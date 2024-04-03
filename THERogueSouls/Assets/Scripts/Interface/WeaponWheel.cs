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

    GameObject[] _spawnedItems = new GameObject[12];

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
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            _itemSlots[i].GetComponent<SpriteRenderer>().sprite = _spawnedItems[i]?.GetComponentInChildren<SpriteRenderer>().sprite;
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
        foreach (GameObject item in _spawnedItems)
        {
            item?.SetActive(false);
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
        for(int i = 0; i < equippedItems.Count; i++)
        {
            string objectToCheck = equippedItems[i].name;
            if (GameObject.Find("Player/Sprite/WeaponHandle/" + equippedItems[i].name) == null)
            {
                Debug.Log("Looking for items to load onto player");
                GameObject spawnedItem = Instantiate(equippedItems[i], _weaponOffsetHandle.transform);
                _spawnedItems[i] = spawnedItem;
            }
        }
    }

    public void PutItemInDesiredSlot(int desiredSlot, int itemToInsert)
    {
        PutAwayCurrentWeapon();
        //_spawnedItems?.RemoveAt(desiredSlot);
        //_spawnedItems.Add(_spawnedItems[itemToInsert]);

        _spawnedItems[desiredSlot] = equippedItems[itemToInsert];
        EquipWeapon(desiredSlot);
    }

    public void EquipWeapon(int index)
    {
        _spawnedItems[index]?.SetActive(true);
        _weaponOffsetHandle.SetCurrentWeapon();
        _playerInputHandler.UpdateRangedWeaponReference();
    }
}
