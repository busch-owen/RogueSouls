using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Character Attributes

    [Header("Character Attributes"), Space(5)]

    [SerializeField]
    public float _xSpeed;

    [SerializeField]
    public float _ySpeed;

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

    [SerializeField]
    float _grappleHookCancelRadius;

    Animator _animator;

    Vector2 _movementSpeed;

    [SerializeField] Rigidbody2D _rb;

    Vector2 _movement;

    bool _rolling = false;
    bool canRoll = true;

    bool _grappling = false;

    [SerializeField]
    int _bonkDamage;
    [SerializeField]
    int _grappleBonkDamage;

    [SerializeField]
    PoolObject _bonkEffect;

    [SerializeField]
    float _bonkKnockback;

    public bool PreventingInput { get; private set; } = false;

    bool _preventingDialogue = false;

    public bool CarryableObjectInRange { get; private set; }
    bool _currentlyCarryingAnObject;
    GameObject _carryableObject;

    [SerializeField]
    float _throwForce;

    [SerializeField]
    float _throwObjectDrag;

    LayerMask _normalMask;
    LayerMask _invulnerableMask;
    LayerMask _grappleMask;

    CharacterInput _characterInput;

    [Space(10)]

    #endregion

    #region Inventory

    Inventory _playerInventory;

    #endregion

    #region Crosshair Attributes

    [Header("CrosshairAttributes"), Space(5)]

    [SerializeField]
    SpriteRenderer _crosshairSprite;
    [SerializeField]
    CrosshairFade _crosshairHandle;

    [SerializeField]
    float _crosshairMoveSpeed;

    Vector2 _crosshairMovement;

    CrosshairClamp _crosshairClamp;

    [Space(10)]

    WaitForFixedUpdate _waitForFixedUpdate = new();

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
    [SerializeField]
    private Transform _gunLocation;

    [field: Space(10)]

    #endregion

    #region Misc Interaction Variables

    public Door CurrentDoor { get; private set; }
    public bool InRangeOfDoor { get; private set; } = false;

    public Chest CurrentChest { get; private set; }
    public bool InRangeOfChest { get; private set; } = false;

    public NPC CurrentNPC { get; private set; }
    public bool InRangeOfNPC { get; private set; } = false;

    HUD _playerHUD;

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
        _crosshairHandle = FindObjectOfType<CrosshairFade>();
        _crosshairSprite = _crosshairHandle.GetComponentInChildren<SpriteRenderer>();
        _weaponOffsetHandle = GetComponentInChildren<WeaponOffsetHandle>();
        _effectHandler = GetComponentInChildren<PlayerEffectHandler>();
        _playerInventory = FindObjectOfType<Inventory>();
        _dodgeSmearRenderer.enabled = false;
        _playerHUD = GetComponentInChildren<HUD>();

        _normalMask = LayerMask.NameToLayer("Player");
        _invulnerableMask = LayerMask.NameToLayer("Invulnerable");
        _grappleMask = LayerMask.NameToLayer("Grapple");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PreventingInput)
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
        if (!_rolling && !_grappling)
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
        _rolling = true;
        _rb.AddForce(_movement.normalized * dodgeRollForce);
        GoInvulnerable(_invulnTime);
        ToggleDashSmear(true);
        yield return new WaitForSeconds(dodgeRollDurationTime);
        StartCoroutine(BeginDodgeRollCoolDown());
        ToggleDashSmear(false);

        _rolling = false;
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
        return _rolling;
    }

    public void PreventInput()
    {
        PreventingInput = true;
        _movementSpeed = Vector2.zero;
        _rb.velocity = Vector2.zero;
    }

    public void AllowInput()
    {
        PreventingInput = false;
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
    

    public void GoGrapple()
    {
        this.gameObject.layer = _grappleMask;
    }

    #endregion

    #region Aiming and Crosshair

    //Manages where the player's weapon should be aimin
    private void HandleAim()
    {
        if(!_crosshairClamp.isActiveAndEnabled)
        {
            _weaponRotationAngle = Mathf.Atan2(_crosshairHandle.transform.position.y, _crosshairHandle.transform.position.x) * Mathf.Rad2Deg;
        }
        else
        {
            _weaponRotationAngle = Mathf.Atan2(_crosshairSprite.transform.position.y - transform.position.y, _crosshairSprite.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        }
        
        Quaternion rotation = Quaternion.AngleAxis(_weaponRotationAngle, Vector3.forward);
        _aimHandleTransform.rotation = Quaternion.Slerp(_aimHandleTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        _weaponOffsetHandle.OffsetWeaponPos(_weaponRotationAngle);
    }

    //Gets the position of the mouse in world space
    public void HandleAimMouseInput(Vector2 aimPostition)
    {
        if (_crosshairClamp != null)
        {
            _crosshairSprite.enabled = false;
            _crosshairClamp.enabled = false;
            aimPostition = Camera.main.ScreenToWorldPoint(aimPostition) - transform.position;
            _crosshairHandle.transform.position = aimPostition;
            _crosshairHandle.FadeInCrosshair();
        } 
    }

    //Handles where the crosshair should go when using a controller
    private void HandleCrosshairControllerMovement()
    {
        _crosshairSprite.transform.localPosition = new Vector3(_crosshairSprite.transform.localPosition.x + _crosshairMovement.x * _crosshairMoveSpeed * Time.fixedDeltaTime,
            _crosshairSprite.transform.localPosition.y + _crosshairMovement.y * _crosshairMoveSpeed * Time.fixedDeltaTime);
        if (_crosshairClamp.enabled)
        {
            _crosshairSprite.enabled = true;
            _crosshairHandle.transform.position = transform.position;
            _crosshairClamp.ClampCrosshair(_crosshairMovement);
            _crosshairHandle.CheckCrosshairPosition();
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
        if(!PreventingInput)
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
    }

    #endregion

    #region Collision Detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>())
        {
            if(_rolling)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(_bonkDamage);
                PoolObject tempParticle = PoolManager.Instance.Spawn(_bonkEffect.name);
                tempParticle.transform.position = (transform.position + collision.transform.position) / 2;
                tempParticle.GetComponent<ParticleSystem>().Play();
            }
            else if(_grappling)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(_grappleBonkDamage);
                PoolObject tempParticle = PoolManager.Instance.Spawn(_bonkEffect.name);
                tempParticle.transform.position = (transform.position + collision.transform.position) / 2;
                tempParticle.GetComponent<ParticleSystem>().Play();
                StopGrappling();
            }
            _rb.velocity = Vector2.zero;
            _rb.AddForce((transform.position - collision.transform.position).normalized * _bonkKnockback, ForceMode2D.Impulse);
        }

        if (_grappling)
            StopGrappling();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carryable" && !_currentlyCarryingAnObject)
        {
            CarryableObjectInRange = true;
            _carryableObject = other.gameObject;
        }
        if (other.gameObject.tag == "Chest" && !InRangeOfChest)
        {
            CurrentChest = other.GetComponent<Chest>();
            InRangeOfChest = true;
        }
        if (other.GetComponent<Door>())
        {
            CurrentDoor = other.GetComponent<Door>();
            InRangeOfDoor = true;
        }
        if (other.GetComponent<NPC>())
        {
            CurrentNPC = other.GetComponent<NPC>();
            CurrentNPC.ResetIndex();
            InRangeOfNPC = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carryable" && !_currentlyCarryingAnObject)
        {
            CarryableObjectInRange = false;
            _carryableObject = null;
        }
        if (other.gameObject.tag == "Chest")
        {
            CurrentChest = null;
            InRangeOfChest = false;
        }
        if (other.GetComponent<Door>())
        {
            CurrentDoor = null;
            InRangeOfDoor = false;
        }
        if (other.GetComponent<NPC>())
        {
            CurrentNPC = null;
            InRangeOfNPC = false;
            _playerHUD.CloseChatBox();
        }
    }
    #endregion

    #region Interactions

    public void Interact()
    {
        if (CarryableObjectInRange && !_currentlyCarryingAnObject)
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

        if (InRangeOfChest)
        {
            CurrentChest.OpenChest();
        }

        if (InRangeOfDoor)
        {
            if (CurrentDoor.IsLocked && _playerInventory.Keys.Count > 0)
            {
                CurrentDoor.UnlockDoor();
                CurrentDoor.OpenDoor();
                _playerInventory.Keys.RemoveAt(0);
            }
            else if (CurrentDoor.IsBossDoor && _playerInventory.BossKeys.Count > 0)
            {
                CurrentDoor.UnlockDoor();
                CurrentDoor.OpenDoor();
                _playerInventory.BossKeys.RemoveAt(0);
            }
        }

        if(InRangeOfNPC && !_preventingDialogue)
        {
            _playerHUD.OpenChatBox();
            CurrentNPC.ContinueDialogue();
        }
        #endregion
    }

    public IEnumerator MoveToSpecificLocation(Vector3 targetPos, float speed, float grappleTime)
    {
        _grappling = true;
        canRoll = false;
        while (_grappling)
        {
            Vector3 moveDirection = targetPos - transform.position;
            _rb.AddForce(moveDirection.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            ToggleDashSmear(true);
            GoGrapple();
            if(Vector2.Distance(targetPos, transform.position) <= _grappleHookCancelRadius)
            {
                StopGrappling();
            }

            yield return _waitForFixedUpdate;
        }
    }

    public void StopGrappling()
    {
        ToggleDashSmear(false);
        GoVulnerable();
        _grappling = false;
        canRoll = true;
    }

    public bool IsGrappling()
    {
        return _grappling;
    }

    public void PreventDialogue()
    {
        _preventingDialogue = true;
        Invoke("AllowDialogue", 3f);
    }

    public void AllowDialogue()
    {
        _preventingDialogue = false;
    }
}