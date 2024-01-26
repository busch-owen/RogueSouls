using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController playerController;

    CharacterInput characterInput;

    private void OnEnable()
    {
        playerController = GetComponent<PlayerController>();
        if(characterInput == null)
        {
            characterInput = new CharacterInput();
            characterInput.CharacterMovement.Movement.performed += i => playerController.HandleMovementInput(i.ReadValue<Vector2>());
            characterInput.CharacterMovement.AimMouse.performed += i => playerController.HandleAimMouseInput(i.ReadValue<Vector2>());
            characterInput.CharacterMovement.AimController.performed += i => playerController.HandleAimControllerInput(i.ReadValue<Vector2>());
            characterInput.CharacterActions.Attack.started += i => playerController.HandleAttackInput();
            characterInput.CharacterActions.DodgeRoll.started += i => playerController.HandleDodgeRollInput();
        }

        characterInput.Enable();
    }
}
