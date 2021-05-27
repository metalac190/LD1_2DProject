using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    // use these strings as a wrapper to interface with InputManager defaults
    // note: Spacebar by default triggers Enter as well...
    public const string SpacebarInputName = "Jump";
    public const string EnterInputName = "Submit";
    public const string EscapeInputName = "Cancel";
    public const string MouseLeftInputName = "Fire1";
    public const string MouseRightInputName = "Fire2";

    public event Action SpacebarPressed;
    public event Action SpacebarReleased;
    public event Action EscapePressed;
    public event Action EscapeReleased;
    public event Action EnterPressed;
    public event Action EnterReleased;
    public event Action MouseLeftPressed;
    public event Action MouseLeftReleased;
    public event Action MouseRightPressed;
    public event Action MouseRightReleased;

    public float Horizontal => Input.GetAxisRaw("Horizontal");
    public float Vertical => Input.GetAxisRaw("Vertical");

    //TODO: access Vector2 as direction
    private Vector2 _inputDirection;
    public Vector2 InputDirection => _inputDirection;

    private void Update()
    {
        // test all input as buttons with string names -> basic controller support
        // spacebar
        if (Input.GetButtonDown(SpacebarInputName))
            SpacebarPressed?.Invoke();
        else if (Input.GetButtonUp(SpacebarInputName))
            SpacebarReleased?.Invoke();
        // escape
        if (Input.GetButtonDown(EscapeInputName))
            EscapePressed?.Invoke();
        else if (Input.GetButtonUp(EscapeInputName))
            EscapeReleased?.Invoke();
        // enter
        if (Input.GetButtonDown(EnterInputName))
            EnterPressed?.Invoke();
        else if (Input.GetButtonUp(EnterInputName))
            EnterReleased?.Invoke();
        // mouse left
        if (Input.GetButtonDown(MouseLeftInputName))
            MouseLeftPressed?.Invoke();
        else if (Input.GetButtonUp(MouseLeftInputName))
            MouseLeftReleased?.Invoke();
        // mouse right
        if (Input.GetButtonDown(MouseRightInputName))
            MouseRightPressed?.Invoke();
        else if (Input.GetButtonUp(MouseRightInputName))
            MouseRightReleased?.Invoke();

        // input vector
        CalculateInputDirection();
    }

    private void CalculateInputDirection()
    {
        _inputDirection = new Vector2(Horizontal, Vertical);
        _inputDirection.Normalize();
    }
}
