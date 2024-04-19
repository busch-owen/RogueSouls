using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;

    [SerializeField]
    GameObject _inventoryMenu, optionsMenu, _gameMenu;

    [SerializeField]
    GameObject _heartsDisplay;

    [SerializeField]
    GameObject _playerStatsPanel;

    [SerializeField]
    Transform _heartDisplayHandlePosition;

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

    [SerializeField]
    TMP_Text _minorSoulCount, _majorSoulCount;

    [SerializeField]
    TMP_Text _smallKeyCount, _bossKeyCount;

    [SerializeField]
    Image _weaponPreviewImage;

    [SerializeField]
    Sprite _emptyHandSprite;

    PlayerStats _playerStats;

    Inventory _playerInventory;

    [SerializeField]
    Image[] _slotImages;

    public bool IsPaused {  get; private set; }

    private void Awake()
    {
        _playerStats = GetComponentInParent<PlayerStats>();
        _playerInventory = FindObjectOfType<Inventory>();
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
        if (!_targetWeapon)
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
        if (!_pauseMenu) return;
        
        if (_pauseMenu.activeSelf)
        {
            IsPaused = false;
            _pauseMenu.SetActive(false);
            ChangeHealthDisplayParent(transform.parent);
            Time.timeScale = 1.0f;
        }
        else
        {
            if (!_targetWeapon)
            {
                _weaponPreviewImage.sprite = _emptyHandSprite;
            }

            IsPaused = true;
            _pauseMenu.SetActive(true);
            _currentMenu = _inventoryMenu;
            OpenSpecificMenu(_currentMenu);
            UpdateItemsCollectedText(_majorSoulCount, _playerStats.MajorSoulsCollected);
            UpdateItemsCollectedText(_minorSoulCount, _playerStats.MinorSoulsCollected);
            UpdateItemsCollectedText(_smallKeyCount, _playerInventory.Keys.Count);
            UpdateItemsCollectedText(_bossKeyCount, _playerInventory.BossKeys.Count);
            ChangeHealthDisplayParent(_heartDisplayHandlePosition);
            Time.timeScale = 0.0f;
        }

    }

    private void ChangeHealthDisplayParent(Transform desiredParent)
    {
        _heartsDisplay.transform.SetParent(desiredParent);
        var heartDisplayTransform = _heartsDisplay.GetComponent<RectTransform>();
        heartDisplayTransform.anchoredPosition = Vector3.zero;
        //heartDisplayTransform.anchorMin = new Vector2(0, -0);
    }

    private void UpdateItemsCollectedText(TMP_Text targetText, int soulAmount)
    {
        targetText.text = soulAmount.ToString();
    }

    public void UpdateWeaponWheelSlots(int index, Sprite image)
    {
        if(image != null)
        {
            _slotImages[index].sprite = image;
            _slotImages[index].SetNativeSize();
        }
        else
        {
            _slotImages[index].sprite = _emptyHandSprite;
            _slotImages[index].SetNativeSize();
        }
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
        _weaponPreviewImage.sprite = _targetWeapon.GetComponentInChildren<SpriteRenderer>().sprite;
        _weaponPreviewImage.SetNativeSize();
    }

    public void EnableReloadingText(float reloadTime)
    {
        _reloadingText.SetActive(true);
        Invoke(nameof(DisableReloadingText), reloadTime);
    }

    private void DisableReloadingText()
    {
        _reloadingText.SetActive(false);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenuTest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
