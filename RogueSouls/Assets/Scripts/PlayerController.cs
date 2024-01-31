using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Attributes"), Space(5)]

    [SerializeField] 
    float _xSpeed;
        
    [SerializeField]
    float _ySpeed;

    [SerializeField]
    float dodgeRollForce;

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

    [Header("CrosshairAttributes"), Space(5)]

    [SerializeField]
    GameObject _crosshair;

    [SerializeField]
    float _crosshairMoveSpeed;

    float _crosshairXMovement;
    float _crosshairYMovement;

    CrosshairClamp _crosshairClamp;

    [Space(10)]

    [Header("Weapon Attributes"), Space(5)]

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    Transform _aimHandleTransform;

    WeaponOffsetHandle _weaponOffsetHandle;

    float _weaponRotationAngle;



    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _crosshairClamp = FindObjectOfType<CrosshairClamp>();
        _weaponOffsetHandle = GetComponentInChildren<WeaponOffsetHandle>();
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

        HandleMovement();
        HandleCrosshairControllerMovement();
    }

    private void HandleMovement()
    {
        if(!rolling)
        {
            _movementSpeed = new Vector2(_xSpeed, _ySpeed);

            _rb.velocity = (_movement).normalized;
            _rb.velocity *= _movementSpeed * Time.fixedDeltaTime;
        }
    }

    private void HandleAim()
    {
        _weaponRotationAngle = Mathf.Atan2(_crosshair.transform.localPosition.y, _crosshair.transform.localPosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_weaponRotationAngle, Vector3.forward);
        _aimHandleTransform.rotation = Quaternion.Slerp(_aimHandleTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        _weaponOffsetHandle.OffsetWeaponPos(_weaponRotationAngle);
    }

    public void HandleMovementInput(Vector2 input)
    {
        _movement = input;

        HandleSpritesAndAnimations();
    }
    public void HandleAimMouseInput(Vector2 aimPosition)
    {
        _crosshairClamp.enabled = false;
        aimPosition = Camera.main.ScreenToWorldPoint(aimPosition) - Camera.main.transform.position;
        _crosshair.transform.localPosition = aimPosition;
    }

    private void HandleCrosshairControllerMovement()
    {
        _crosshair.transform.localPosition = new Vector3(_crosshair.transform.localPosition.x + _crosshairXMovement * _crosshairMoveSpeed * Time.fixedDeltaTime, 
            _crosshair.transform.localPosition.y + _crosshairYMovement * _crosshairMoveSpeed * Time.fixedDeltaTime);
    }

    public void HandleAimControllerInput(Vector2 aimPosition)
    {
        _crosshairClamp.enabled = true;
        _crosshairXMovement = aimPosition.x;
        _crosshairYMovement = aimPosition.y;
    }

    public void HandleDodgeRollInput()
    {
        if(canRoll && _rb.velocity != Vector2.zero)
        {
            _animator.SetTrigger("Dodge");
            StartCoroutine(BeginDodgeRollDuration());
            canRoll = false; 
        }
        
    }

    public void HandleSpritesAndAnimations()
    {
        if (_movement.x != 0 || _movement.y != 0)
        {
            _animator.SetBool("Running", true);
            //_animator.speed = (Mathf.Abs(_movement.x) + Mathf.Abs(_movement.y)) / 2;
        }
        else
        {
            _animator.SetBool("Running", false);
            //_animator.speed = 1;
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

    public void HandleAttackInput()
    {
        //Attack code goes here...
        Debug.Log("Attack");
    }

    IEnumerator BeginDodgeRollDuration()
    {
        rolling = true;
        _rb.AddForce(_movement.normalized * dodgeRollForce);
        yield return new WaitForSeconds(dodgeRollDurationTime);
        StartCoroutine(BeginDodgeRollCoolDown());
        rolling = false;
    }

    IEnumerator BeginDodgeRollCoolDown()
    {
        yield return new WaitForSeconds(dodgeRollCooldownTime);
        canRoll = true;
    }
}
