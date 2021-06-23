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

    public bool MoveHeld { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool MenuHeld { get; private set; }
    public bool DashHeld { get; private set; }
    public bool AttackHeld { get; private set; }

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
        {
            MoveHeld = true;
            MovementPressed?.Invoke();
        }
        else if (context.canceled)
        {
            MoveHeld = false;
            MovementCancelled?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpHeld = true;
            JumpPressed?.Invoke();
        }  
        else if (context.canceled)
        {
            JumpHeld = false;
            JumpReleased?.Invoke();
        }   
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashHeld = true;
            DashPressed?.Invoke();
        }
        else if (context.canceled)
        {
            DashHeld = false;
            DashReleased?.Invoke();
        }
            
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackHeld = true;
            AttackPressed?.Invoke();
        }
            
        else if (context.canceled)
        {
            AttackHeld = false;
            AttackReleased?.Invoke();
        }
            
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MenuHeld = true;
            MenuPressed?.Invoke();
        }
            
        else if (context.canceled)
        {
            MenuHeld = false;
            MenuReleased?.Invoke();
        }  
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }
}
