using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This script is responsible for detecting ground underneath this object.
/// Define a ground check location - if you'd rather know the moment when grounded
/// changes, then you can subscribe to the events. If you'd rather just occasionally read
/// the current state of Grounded, just read the variable.
/// </summary>
public class GroundDetector : MonoBehaviour
{
    public event Action FoundGround;    // previously no ground, and now there is
    public event Action LeftGround;     // previously ground, and now there's not

    [SerializeField]
    private float _groundCheckRadius = .3f;
    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private Transform _groundCheckLocation;

    public float TimeInAir { get; private set; } = 0;
    public float TimeOnGround { get; private set; } = 0;

    // note: this variable will only get updated based on last check. This doesn't happen automatically,
    // you must call it using the Detect() methods. This is intended so we can save detect calls and use
    // them specifically when we want them.
    private bool _isGrounded = false;
    public bool IsGrounded {
        get => _isGrounded;
        private set
        {
            // if our grounded state is about to change
            if(value != _isGrounded)
            {
                // if we're about to be grounded, we're just now landing
                if (value == true)
                {
                    TimeInAir = 0;
                    FoundGround?.Invoke();
                } 
                // if we're about to be airborn, we're just now taking off
                else if (value == false)
                {
                    TimeOnGround = 0;
                    LeftGround?.Invoke();
                }
            }
            _isGrounded = value;
        }
    }

    private void Update()
    {
        // track Time spent on ground/air
        if (IsGrounded)
            TimeOnGround += Time.deltaTime;
        else
            TimeInAir += Time.deltaTime;
    }

    public bool DetectGround()
    {
        if(_groundCheckLocation != null)
        {
            IsGrounded = Physics2D.OverlapCircle(_groundCheckLocation.position, 
                _groundCheckRadius, _whatIsGround);
            return IsGrounded;
        }
        else
        {
            Debug.LogWarning("No groundcheck specified on: " + gameObject.name);
            return false;
        }
        
    }

    private void OnDrawGizmos()
    {
        if(_groundCheckLocation != null)
        {
            Gizmos.DrawWireSphere(_groundCheckLocation.position, _groundCheckRadius);
        }
    }
}
