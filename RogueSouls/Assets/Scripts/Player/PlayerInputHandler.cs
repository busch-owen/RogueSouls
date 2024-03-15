using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController playerController;
    private RangedWeapon rangedWeapon;
    private MeleeBase meleeWeapon;
    private WeaponWheel weaponWheel;
    private UIHandler uiHandler;

    CharacterInput characterInput;

    private void OnEnable()
    {
        playerController = GetComponent<PlayerController>();
        uiHandler = FindObjectOfType<UIHandler>();
        meleeWeapon = FindObjectOfType<MeleeBase>();
        UpdateRangedWeaponReference();
        weaponWheel = FindObjectOfType<WeaponWheel>();
        if(characterInput == null)
        {
            if(!uiHandler.IsPaused)
            {
                characterInput = new CharacterInput();
                characterInput.CharacterMovement.Movement.performed += i => playerController?.HandleMovementInput(i.ReadValue<Vector2>());
                characterInput.CharacterMovement.AimMouse.performed += i => playerController?.HandleAimMouseInput(i.ReadValue<Vector2>());
                characterInput.CharacterMovement.AimController.performed += i => playerController?.HandleAimControllerInput(i.ReadValue<Vector2>());

                characterInput.CharacterMovement.AimMouse.performed += i => weaponWheel?.HandleArrowInputMouse(i.ReadValue<Vector2>());
                characterInput.CharacterMovement.AimController.performed += i => weaponWheel?.HandleArrowInputController(i.ReadValue<Vector2>());

                characterInput.CharacterActions.Attack.started += i => rangedWeapon?.EnableShootInput();
                characterInput.CharacterActions.Attack.canceled += i => rangedWeapon?.DisableShootInput();
                characterInput.CharacterActions.MeleeAttack.started += i => meleeWeapon.Attack();
                characterInput.CharacterActions.DodgeRoll.started += i => playerController?.HandleDodgeRollInput();
                characterInput.CharacterActions.Interact.started += i => playerController?.Interact();

                characterInput.CharacterActions.OpenWeaponWheel.started += i => weaponWheel?.OpenWeaponWheel();
                characterInput.CharacterActions.OpenWeaponWheel.canceled += i => weaponWheel?.CloseWeaponWheel();
            }
            
            characterInput.CharacterActions.PauseMenu.started += i => uiHandler.TogglePauseMenu();
        }

        characterInput.Enable();
    }

    public void UpdateRangedWeaponReference()
    {
        rangedWeapon = GetComponentInChildren<RangedWeapon>();
    }
}
