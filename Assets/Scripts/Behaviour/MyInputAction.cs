using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInputAction : MonoBehaviour
{
    public static PlayerAction Input { get; private set; }

    public static float Move { get; private set; }
    public static bool MoveBindings { get; private set; }
    public static bool Pause { get; private set; }

    void Awake()
    {
        Input = new();
        Input.Buttons.Movement.started += OnMovementInput;
        Input.Buttons.Movement.performed += OnMovementInput;
        Input.Buttons.Movement.canceled += OnMovementInput;
        Input.Buttons.MovementBindings.started += OnMovementBindingsInput;
        Input.Buttons.MovementBindings.canceled += OnMovementBindingsInput;
        Input.Buttons.Pause.started += OnPauseInput;
        Input.Buttons.Pause.canceled += OnPauseInput;
    }

    void OnMovementInput(InputAction.CallbackContext ctx)
    {
        Vector2 move = ctx.ReadValue<Vector2>();
        Move = move.x;
        if (Move > 0f) Move = 1f;
        if (Move < 0f) Move = -1f;
    }

    void OnMovementBindingsInput(InputAction.CallbackContext ctx)
    {
        MoveBindings = ctx.ReadValueAsButton();
    }

    void OnPauseInput(InputAction.CallbackContext ctx)
    {
        Pause = ctx.ReadValueAsButton();
    }

    void LateUpdate()
    {
        MoveBindings = false;
        Pause = false;
    }

    void OnEnable()
    {
        Input.Buttons.Enable();
    }

    void OnDisable()
    {
        Input.Buttons.Disable();
    }
}