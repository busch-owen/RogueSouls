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
    }

    private void HandleArrowRotation()
    {
        _arrowAngle = Mathf.Atan2(_aimPosition.y, _aimPosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_arrowAngle, Vector3.forward);
        _arrow.rotation = Quaternion.Slerp(_arrow.rotation, rotation, _rotateSpeed * Time.deltaTime);
    }

    public void HandleArrowInputMouse(Vector2 aimPosition)
    {
        aimPosition = _camera.ScreenToWorldPoint(aimPosition) - _camera.transform.position;
        _aimPosition = aimPosition;
    }
    public void HandleArrowInputController(Vector2 aimPosition)
    {
        _aimPosition = aimPosition;
    }

    public void OpenWeaponWheel()
    {
        _weaponWheelObject.SetActive(true);
        FillItemSlots();
        Time.timeScale = _menuOpenTimeScale;
    }

    public void CloseWeaponWheel()
    {
        _weaponWheelObject.SetActive(false);
        PutAwayCurrentWeapon();
        _playerWeapons[WhichWeaponToSelect()].gameObject.SetActive(true);
        _playerInputHandler.UpdateRangedWeaponReference();
        _weaponOffsetHandle.SetCurrentWeapon();
        Time.timeScale = 1;
    }

    private void FillItemSlots()
    {
        for(int i = 0; i < _itemSlots.Length; i++)
        {
            if (_weapons[i] != null)
            {
                _itemSlots[i].GetComponent<SpriteRenderer>().sprite = _weapons[i].GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                _itemSlots[i].GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    private int WhichWeaponToSelect()
    {
        if (_arrowAngle is < 135 and >= 45)
            return 0;
        else if (_arrowAngle is < 45 and >= -45)
            return 1;
        else if (_arrowAngle is < -45 and >= -135)
            return 2;
        else
            return 3;
    }

    private void PutAwayCurrentWeapon()
    {
        for (int i = 0; i < _playerWeapons.Length; i++)
        {
            _playerWeapons[i].gameObject.SetActive(false);
        }
    }
}
