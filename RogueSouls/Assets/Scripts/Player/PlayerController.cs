using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Character Attributes

    [Header("Character Attributes"), Space(5)]

    [SerializeField]
    float _xSpeed;

    [SerializeField]
    float _ySpeed;

    [SerializeField]
    float dodgeRollForce;

    [SerializeField]
    float playerFriction;

    [SerializeField]
    float dodgeRollCooldownTime;

    [SerializeField]
    float dodgeRollDurationTime;

    [SerializeField]
    GameObject _playerSpriteObject;

    [SerializeField]
    GameObject _objectCarryPoint;

    [SerializeField]
    float _invulnTime;

    Animator _animator;

    Vector2 _movementSpeed;

    [SerializeField] Rigidbody2D _rb;

    Vector2 _movement;

    bool rolling = false;
    bool canRoll = true;

    bool _preventInput = false;

    bool _carryableObjectInRange;
    bool _currentlyCarryingAnObject;
    GameObject _carryableObject;

    [SerializeField]
    float _throwForce;

    [SerializeField]
    float _throwObjectDrag;

    LayerMask _normalMask;
    LayerMask _invulnerableMask;

    [Space(10)]

    #endregion

    #region Inventory

    Inventory _playerInventory;

    #endregion

    #region Crosshair Attributes

    [Header("CrosshairAttributes"), Space(5)]

    [SerializeField]
    GameObject _crosshair;

    [SerializeField]
    float _crosshairMoveSpeed;

    Vector2 _crosshairMovement;

    CrosshairClamp _crosshairClamp;

    [Space(10)]

    #endregion

    #region Weapon Attributes

    [Header("Weapon Attributes"), Space(5)]

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    Transform _aimHandleTransform;

    WeaponOffsetHandle _weaponOffsetHandle;

    float _weaponRotationAngle;

    [SerializeField]
    private RangedWeapon _gun;
    private MeleeBase _melee;
    [SerializeField]
    private Transform _gunLocation;

    [Space(10)]

    #endregion

    #region Misc Interaction Variables

    Door _currentDoor;
    bool _inRangeOfDoor = false;

    Chest _currentChest;
    bool _inRangeOfChest = false;

    #endregion

    #region Effects

    [Header("Effects"), Space(5)]

    PlayerEffectHandler _effectHandler;

    [SerializeField]
    TrailRenderer _dodgeSmearRenderer;

    //[Space(10)]

    #endregion

    #region Unity Runtime Functions

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _crosshairClamp = FindObjectOfType<CrosshairClamp>();
        _crosshair = _crosshairClamp.gameObject;
        _weaponOffsetHandle = GetComponentInChildren<WeaponOffsetHandle>();
        _effectHandler = GetComponentInChildren<PlayerEffectHandler>();
        _playerInventory = FindObjectOfType<Inventory>();
        _dodgeSmearRenderer.enabled = false;
        _melee = GetComponentInChildren<MeleeBase>();

        _normalMask = LayerMask.NameToLayer("Player");
        _invulnerableMask = LayerMask.NameToLayer("Invulnerable");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_preventInput)
        {
            HandleAim();
            HandleMovement();
        }
        HandleCrosshairControllerMovement();
    }

    private void FixedUpdate()
    {
        HandleSpritesAndAnimations();


        if (_currentlyCarryingAnObject)
        {
            _carryableObject.transform.localPosition = Vector2.zero;
        }
    }

    #endregion

    #region Movement Input and Physics
    private void HandleMovement()
    {
        if (!rolling && !_melee.IsLunging())
        {
            _movementSpeed = new Vector2(_xSpeed, _ySpeed);

            _rb.velocity = PlayerMovement();
        }
    }

    public void HandleMovementInput(Vector2 input)
    {
        _movement = input;
    }
    public void HandleDodgeRollInput()
    {
        if(_rb != null)
        {
            if (canRoll && _rb.velocity != Vector2.zero)
            {
                _animator.SetTrigger("Dodge");
                StartCoroutine(BeginDodgeRollDuration());
                canRoll = false;
            }
        }
    }

    Vector2 PlayerMovement()
    {
        return Vector2.Lerp(_rb.velocity, _movement.normalized * _movementSpeed * Time.fixedDeltaTime, playerFriction);
    }

    IEnumerator BeginDodgeRollDuration()
    {
        rolling = true;
        _rb.AddForce(_movement.normalized * dodgeRollForce);
        GoInvulnerable(_invulnTime);
        ToggleDashSmear(true);
        yield return new WaitForSeconds(dodgeRollDurationTime);
        StartCoroutine(BeginDodgeRollCoolDown());
        ToggleDashSmear(false);

        rolling = false;
    }

    IEnumerator BeginDodgeRollCoolDown()
    {
        yield return new WaitForSeconds(dodgeRollCooldownTime);
        canRoll = true;
    }

    public void ToggleDashSmear(bool state)
    {
        _dodgeSmearRenderer.enabled = state;
    }

    public bool CurrentlyRolling()
    {
        return rolling;
    }

    public void PreventInput()
    {
        _preventInput = true;
        _movementSpeed = Vector2.zero;
        _rb.velocity = Vector2.zero;
    }

    public void AllowInput()
    {
        _preventInput = false;
    }

    public void GoInvulnerable(float invTime)
    {
        this.gameObject.layer = _invulnerableMask;
        Invoke("GoVulnerable", invTime);
    }

    public void GoVulnerable()
    {
        this.gameObject.layer = _normalMask;
    }

    #endregion

    #region Aiming and Crosshair

    //Manages where the player's weapon should be aimin
    private void HandleAim()
    {
        _weaponRotationAngle = Mathf.Atan2(_crosshair.transform.localPosition.y, _crosshair.transform.localPosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_weaponRotationAngle, Vector3.forward);
        _aimHandleTransform.rotation = Quaternion.Slerp(_aimHandleTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        _weaponOffsetHandle.OffsetWeaponPos(_weaponRotationAngle);
    }

    //Gets the position of the mouse in world space
    public void HandleAimMouseInput(Vector2 aimPosition)
    {
        if (_crosshairClamp != null)
        {
            _crosshairClamp.enabled = false;
            aimPosition = Camera.main.ScreenToWorldPoint(aimPosition) - Camera.main.transform.position;
            _crosshair.transform.localPosition = aimPosition;
        }
        
    }

    //Handles where the crosshair should go when using a controller
    private void HandleCrosshairControllerMovement()
    {
        _crosshair.transform.localPosition = new Vector3(_crosshair.transform.localPosition.x + _crosshairMovement.x * _crosshairMoveSpeed * Time.fixedDeltaTime,
            _crosshair.transform.localPosition.y + _crosshairMovement.y * _crosshairMoveSpeed * Time.fixedDeltaTime);
        if (_crosshairClamp.enabled)
        {
            _crosshairClamp.ClampCrosshair(_crosshairMovement);
        }
    }

    public void HandleAimControllerInput(Vector2 aimPosition)
    {
        _crosshairClamp.enabled = true;
        _crosshairMovement = aimPosition;
    }

    #endregion

    #region Animations and Effects

    //All the animation handling happens here...
    public void HandleSpritesAndAnimations()
    {
        if (_movement.x != 0 || _movement.y != 0)
        {
            _animator.SetBool("Running", true);
            if (!_effectHandler.RunParticlesPlaying())
                _effectHandler.PlayRunParticles();
        }
        else
        {
            _animator.SetBool("Running", false);
            _effectHandler.StopRunParticles();
        }

        if (_movement.x < 0)
        {
            _playerSpriteObject.transform.localScale = new Vector2(-1, 1);
        }
        else if (_movement.x > 0)
        {
            _playerSpriteObject.transform.localScale = new Vector2(1, 1);
        }
    }

    #endregion

    #region Collision Detection

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carryable" && !_currentlyCarryingAnObject)
        {
            _carryableObjectInRange = true;
            _carryableObject = other.gameObject;
        }
        if (other.gameObject.tag == "Chest" && !_inRangeOfChest)
        {
            _currentChest = other.GetComponent<Chest>();
            _inRangeOfChest = true;
        }
        if (other.GetComponent<Door>())
        {
            _currentDoor = other.GetComponent<Door>();
            _inRangeOfDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carryable" && !_currentlyCarryingAnObject)
        {
            _carryableObjectInRange = false;
            _carryableObject = null;
        }
        if (other.gameObject.tag == "Chest")
        {
            _currentChest = null;
            _inRangeOfChest = false;
        }
        if (other.GetComponent<Door>())
        {
            _currentDoor = null;
            _inRangeOfDoor = false;
        }
    }
    #endregion

    #region Interactions

    public void Interact()
    {
        if (_carryableObjectInRange && !_currentlyCarryingAnObject)
        {
            BoxCollider2D heldCollider = _carryableObject.GetComponent<BoxCollider2D>();
            heldCollider.enabled = false;
            _aimHandleTransform.gameObject.SetActive(false);
            _carryableObject.transform.parent = _objectCarryPoint.transform;
            _carryableObject.transform.localPosition = Vector3.zero;
            _currentlyCarryingAnObject = true;
        }
        else if (_currentlyCarryingAnObject)
        {
            Rigidbody2D tempRb = _carryableObject.GetComponent<Rigidbody2D>();
            tempRb.velocity = new Vector2(_throwForce + (Mathf.Abs(_rb.velocity.x) * 2), tempRb.velocity.y) * _playerSpriteObject.transform.localScale.x;
            _aimHandleTransform.gameObject.SetActive(true);
            _carryableObject.transform.parent = null;
            _currentlyCarryingAnObject = false;
            BoxCollider2D heldCollider = _carryableObject.GetComponent<BoxCollider2D>();
            heldCollider.enabled = true;
        }

        if (_inRangeOfChest)
        {
            _currentChest.OpenChest();
        }

        if (_inRangeOfDoor)
        {
            if (_currentDoor.IsLocked && _playerInventory.Keys.Count > 0)
            {
                _currentDoor.UnlockDoor();
                _currentDoor.OpenDoor();
                _playerInventory.Keys.RemoveAt(0);
            }
            else if (_currentDoor.IsBossDoor && _playerInventory.BossKeys.Count > 0)
            {
                _currentDoor.UnlockDoor();
                _currentDoor.OpenDoor();
                _playerInventory.BossKeys.RemoveAt(0);
            }
        }
        #endregion
    }
}