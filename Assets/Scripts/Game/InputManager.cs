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

    public event Action LeftPressed;
    public event Action RightPressed;
    public event Action UpPressed;
    public event Action DownPressed;

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

    public bool SpacebarHeld => Input.GetButton(SpacebarInputName);
    public bool EscapeHeld => Input.GetButton(EscapeInputName);
    public bool EnterHeld => Input.GetButton(EnterInputName);
    public bool MouseLeftHeld => Input.GetButton(MouseLeftInputName);
    public bool MouseRightHeld => Input.GetButton(MouseRightInputName);

    private Vector2 _direction;
    public Vector2 Direction => _direction;

    // communicate when new left/right directions are received
    private float _xRaw;
    public float XRaw
    {
        get => _xRaw;
        private set
        {
            if(value != _xRaw)
            {
                if(value == 1)
                    RightPressed?.Invoke();
                else if(value == -1)
                    LeftPressed?.Invoke();
            }
            _xRaw = value;
        }
    }
    // communicate when new up/down directions are received
    private float _yRaw;
    public float YRaw
    {
        get => _yRaw;
        private set
        {
            if (value != _yRaw)
            {
                if (value == 1)
                    UpPressed?.Invoke();
                else if (value == -1)
                    DownPressed?.Invoke();
            }
            _yRaw = value;
        }
    }

    //TODO: add input buffers for better User Experience, if we need it, using time since last input

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
        XRaw = Input.GetAxisRaw("Horizontal");
        YRaw = Input.GetAxisRaw("Vertical");

        _direction = new Vector2(XRaw, YRaw);
        _direction.Normalize();
    }
}
