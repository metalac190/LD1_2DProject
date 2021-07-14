using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// This class is just a wrapper class to make navigation input easier to deal with.
/// Author: Adam Chandler
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class MenuInput : MonoBehaviour
{
    public event Action ClosePressed;
    public event Action CloseReleased;

    public event Action CancelPressed;
    public event Action CancelReleased;

    public event Action SubmitPressed;
    public event Action SubmitReleased;

    public event Action NavigatePressed;
    public event Action NavigateReleased;

    public event Action PointPressed;
    public event Action PointReleased;

    public bool SubmitHeld { get; private set; }
    public bool CancelHeld { get; private set; }

    //TODO: Make navigation functional for UI

    public void OnClose(InputAction.CallbackContext context)
    {
        //Debug.Log("Input: Close");
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        //Debug.Log("Input: Cancel");
        if (context.started)
        {
            CancelHeld = true;
            CancelPressed?.Invoke();
        }
        else if (context.canceled)
        {
            CancelHeld = false;
            CancelReleased?.Invoke();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SubmitHeld = true;
            SubmitPressed?.Invoke();
        }
        else if (context.canceled)
        {
            SubmitHeld = false;
            SubmitReleased?.Invoke();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        //Debug.Log("Input: Navigate");
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        //Debug.Log("Input: Point");
    }
}
