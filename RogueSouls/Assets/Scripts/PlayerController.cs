using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xMovement;
    float yMovement;

    [SerializeField]
    float crosshairMoveSpeed;

    float crosshairXMovement;
    float crosshairYMovement;

    [SerializeField]
    GameObject crosshair;

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    Transform _aimHandleTransform;

    WeaponOffsetHandle _weaponOffsetHandle;

    float _weaponRotationAngle;

    CrosshairClamp _crosshairClamp;

    // Start is called before the first frame update
    private void Awake()
    {
        _crosshairClamp = GetComponentInChildren<CrosshairClamp>();
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
        HandleMovement();
        HandleAim();
        HandleCrosshairControllerMovement();
    }

    private void HandleMovement()
    {

    }

    private void HandleAim()
    {
        _weaponRotationAngle = Mathf.Atan2(crosshair.transform.position.y, crosshair.transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_weaponRotationAngle, Vector3.forward);
        _aimHandleTransform.rotation = Quaternion.Slerp(_aimHandleTransform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        _weaponOffsetHandle.OffsetWeaponPos(_weaponRotationAngle);
    }

    public void HandleMovementInput(Vector2 input)
    {
        xMovement = input.x;
        yMovement = input.y;
    }
    public void HandleAimMouseInput(Vector2 aimPosition)
    {
        _crosshairClamp.enabled = false;
        aimPosition = Camera.main.ScreenToWorldPoint(aimPosition) - transform.position;
        crosshair.transform.position = aimPosition;
    }

    private void HandleCrosshairControllerMovement()
    {
        crosshair.transform.position = new Vector2(crosshair.transform.position.x + crosshairXMovement * crosshairMoveSpeed * Time.fixedDeltaTime, 
            crosshair.transform.position.y + crosshairYMovement * crosshairMoveSpeed * Time.fixedDeltaTime);
    }

    public void HandleAimControllerInput(Vector2 aimPosition)
    {
        _crosshairClamp.enabled = true;
        crosshairXMovement = aimPosition.x;
        crosshairYMovement = aimPosition.y;
    }

    public void HandleDodgeRollInput()
    {
        //Dodge roll code goes here...
        Debug.Log("Roll");
    }

    public void HandleAttackInput()
    {
        //Attack code goes here...
        Debug.Log("Attack");
    }
}
