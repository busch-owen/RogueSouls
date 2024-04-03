using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;

    [SerializeField]
    GameObject _inventoryMenu;

    [SerializeField]
    GameObject _heartsDisplay;

    GameObject _currentMenu;

    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        _pauseMenu.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if(_pauseMenu == null)
        {
            return;
        }
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
}
