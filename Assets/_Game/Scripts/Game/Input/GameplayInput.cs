using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// This class is just a wrapper for Unity's input system to make it easier to work with
/// inside of multiple states that need to listen/stop listening to input.
/// It requires the PlayerInput component in order to function so that we can add
/// multiplayer later on, if we wish.
/// Author: Adam Chandler
/// </summary>

[RequireComponent(typeof(PlayerInput))]
public class GameplayInput : MonoBehaviour
{
    public event Action MovementPressed;
    public event Action MovementCancelled;

    public event Action JumpPressed;
    public event Action JumpReleased;

    public event Action MenuPressed;
    public event Action MenuReleased;

    public event Action DashPressed;
    public event Action DashReleased;

    public event Action AttackPressed;
    public event Action AttackReleased;

    public Vector2 MoveInput { get; private set; }

    public float XInput => MoveInput.x;
    public float YInput => MoveInput.y;
    public int XInputRaw { get; private set; }
    public int YInputRaw { get; private set; }

    public Vector2 MousePosition { get; private set; }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        XInputRaw = Mathf.RoundToInt(MoveInput.x);
        YInputRaw = Mathf.RoundToInt(MoveInput.y);

        if (context.started)
            MovementPressed?.Invoke();
        else if (context.canceled)
            MovementCancelled?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            JumpPressed?.Invoke();
        else if (context.canceled)
            JumpReleased?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
            DashPressed?.Invoke();
        else if (context.canceled)
            DashReleased?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            AttackPressed?.Invoke();
        else if (context.canceled)
            AttackReleased?.Invoke();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.started)
            MenuPressed?.Invoke();
        else if (context.canceled)
            MenuReleased?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }
}
