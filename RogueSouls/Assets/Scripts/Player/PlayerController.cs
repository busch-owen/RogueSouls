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

    Animator _animator;

    Vector2 _movementSpeed;

    [SerializeField] Rigidbody2D _rb;

    Vector2 _movement;

    bool rolling = false;
    bool canRoll = true;

    [Space(10)]

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
	private RangedWeapon gun;
	[SerializeField]
	private Transform gunLocation;

    [Space(10)]

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
        _weaponOffsetHandle = GetComponentInChildren<WeaponOffsetHandle>();
        _effectHandler = GetComponentInChildren<PlayerEffectHandler>();
        _dodgeSmearRenderer.enabled = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        HandleAim();
        HandleSpritesAndAnimations();
        HandleMovement();
        HandleCrosshairControllerMovement();
    }

    #endregion

    #region Movement Input and Physics
    private void HandleMovement()
    {
        if(!rolling)
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
        if (canRoll && _rb.velocity != Vector2.zero)
        {
            _animator.SetTrigger("Dodge");
            StartCoroutine(BeginDodgeRollDuration());
            canRoll = false;
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
        _dodgeSmearRenderer.enabled = true;
        yield return new WaitForSeconds(dodgeRollDurationTime);
        StartCoroutine(BeginDodgeRollCoolDown());
        _dodgeSmearRenderer.enabled = false;
        rolling = false;
    }

    IEnumerator BeginDodgeRollCoolDown()
    {
        yield return new WaitForSeconds(dodgeRollCooldownTime);
        canRoll = true;
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
        _crosshairClamp.enabled = false;
        aimPosition = Camera.main.ScreenToWorldPoint(aimPosition) - Camera.main.transform.position;
        _crosshair.transform.localPosition = aimPosition;
    }

    //Handles where the crosshair should go when using a controller
    private void HandleCrosshairControllerMovement()
    {
        _crosshair.transform.localPosition = new Vector3(_crosshair.transform.localPosition.x + _crosshairMovement.x * _crosshairMoveSpeed * Time.fixedDeltaTime, 
            _crosshair.transform.localPosition.y + _crosshairMovement.y * _crosshairMoveSpeed * Time.fixedDeltaTime);
        if(_crosshairClamp.enabled)
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
            if(!_effectHandler.RunParticlesPlaying())
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
}
