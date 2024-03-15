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

    Camera _camera;

    Vector2 _aimPosition;

    [SerializeField]
    ItemHighlight _itemHighlight;

    [SerializeField]
    Transform[] _itemSlots;

    [SerializeField]
    RangedWeapon[] _weapons;

    RangedWeapon[] _playerWeapons;

    PlayerController _playerController;

    WeaponOffsetHandle _weaponOffsetHandle;

    PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        _camera = Camera.main;
        _playerController = FindObjectOfType<PlayerController>();
        _weaponOffsetHandle = _playerController.GetComponentInChildren<WeaponOffsetHandle>();
        _playerInputHandler = _playerController.GetComponent<PlayerInputHandler>();
        _weaponWheelObject.SetActive(false);

        _weapons = _playerController.GetComponentsInChildren<RangedWeapon>();
        _playerWeapons = _playerController.GetComponentsInChildren<RangedWeapon>();

        PutAwayCurrentWeapon();

        _playerWeapons[0].gameObject.SetActive(true);
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
        if(_weaponWheelObject != null )
        {
            _weaponWheelObject.SetActive(false);
            if (WhichWeaponToSelect() < _playerWeapons.Length)
            {
                PutAwayCurrentWeapon();
                _playerWeapons[WhichWeaponToSelect()].gameObject.SetActive(true);
                _playerInputHandler.UpdateRangedWeaponReference();
                _weaponOffsetHandle.SetCurrentWeapon();
            }
            Time.timeScale = 1;
        }
    }

    private void FillItemSlots()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _itemSlots[i].GetComponent<SpriteRenderer>().sprite = _weapons[i].GetComponentInChildren<SpriteRenderer>().sprite;
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
        for (int i = 0; i < _playerWeapons.Length; i++)
        {
            _playerWeapons[i].gameObject.SetActive(false);
        }
    }
}
