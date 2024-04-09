using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;

    [SerializeField]
    GameObject _inventoryMenu, optionsMenu, _gameMenu;

    [SerializeField]
    GameObject _heartsDisplay;

    GameObject _currentMenu;

    [SerializeField]
    GameObject _ammoLayout;
    [SerializeField]
    TMP_Text _ammoCounter;
    [SerializeField]
    TMP_Text _maxAmmoNumber;
    RangedWeapon _targetWeapon;

    [SerializeField]
    GameObject _reloadingText;

    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        /*
        _pauseMenu.SetActive(false);
        _inventoryMenu.SetActive(false);
        _gameMenu.SetActive(false);
        */
    }

    private void Start()
    {
        _pauseMenu.SetActive(false);
        _inventoryMenu.SetActive(false);
        _gameMenu.SetActive(false);
        _reloadingText.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_targetWeapon == null)
        {
            _ammoLayout.SetActive(false);
        }
        else
        {
            _ammoLayout.SetActive(true);
            _ammoCounter.text = _targetWeapon.CurrentAmmo.ToString();
            _maxAmmoNumber.text = "/ " + _targetWeapon.MaxAmmo.ToString();
        }
    }

    public void TogglePauseMenu()
    {
        if(_pauseMenu != null)
        {
            if (_pauseMenu.activeSelf)
            {
                _pauseMenu.SetActive(false);
                ChangeHealthDisplayState(true);
                Time.timeScale = 1.0f;
            }
            else
            {
                _pauseMenu.SetActive(true);
                _currentMenu = _inventoryMenu;
                OpenSpecificMenu(_currentMenu);
                ChangeHealthDisplayState(false);
                Time.timeScale = 0.0f;
            }
        }
        
    }

    private void ChangeHealthDisplayState(bool desiredState)
    {
        _heartsDisplay.SetActive(desiredState);
    }

    public void OpenSpecificMenu(GameObject menuToOpen)
    {
        _currentMenu.SetActive(false);
        _currentMenu = menuToOpen;
        _currentMenu.SetActive(true);
    }

    public void AssignTargetWeapon(RangedWeapon weapon)
    {
        _targetWeapon = weapon;
    }

    public void EnableReloadingText(float reloadTime)
    {
        _reloadingText.SetActive(true);
        Invoke("DisableReloadingText", reloadTime);
    }

    void DisableReloadingText()
    {
        _reloadingText.SetActive(false);
    }
}
