using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour, IInteractInput, IMoveInput, ISkillInput, IPickUpInput
{
    // Start is called before the first frame update
    public Command interactInput;
    public Command movementInput;
    public Command skillInput;
    public Command pickUpInput;

    private PlayerInputActions thisInputActions;
    private CharacterController thisCharacterController;

    public bool thisDashing;


    public Vector3 MoveDirection { get; private set; }
    public bool IsUsingSkill { get; private set; }
    public bool IsPressingInteract { get; private set; }

    public bool IsPressingPickUp { get; private set; }

    private void Awake()
    {
        thisInputActions = new PlayerInputActions();
        thisCharacterController = GetComponent<CharacterController>();
        thisDashing = false;
    }

    private void OnEnable()
    {
        thisInputActions.Enable();

        if (movementInput)
        {
                thisInputActions.Player.Movement.performed += OnMoveInput;
        }


        thisInputActions.Player.Interact.performed += OnInteractButton;

        thisInputActions.Player.PickUp.performed += OnPickUpButton;

        if (skillInput)
            thisInputActions.Player.Dash.performed += OnSkillButton;
    }
    

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();


        MoveDirection = new Vector3(value.x, 0, value.y);
        if (movementInput != null)
            movementInput.Execute();
    }

    private void OnInteractButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingInteract = value >= 0.15f;
        if (interactInput != null && IsPressingInteract)
            interactInput.Execute();
    }

    private void OnPickUpButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingPickUp = value >= 0.15f;
        if (pickUpInput != null && IsPressingPickUp)
            pickUpInput.Execute();
    }


    private void OnSkillButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsUsingSkill = value >= 0.15f;
        if (skillInput != null && IsUsingSkill && thisDashing == false)
            skillInput.Execute();
    }



    private void OnDisable()
    {
        thisInputActions.Player.Interact.performed -= OnInteractButton;

        thisInputActions.Disable();
    }
}
