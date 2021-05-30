using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallDetector : MonoBehaviour
{
    public event Action FoundWall;
    public event Action LostWall;

    [SerializeField]
    private Transform _wallCheckLocation;
    [SerializeField]
    private float _wallCheckDistance = 0.5f;
    [SerializeField]
    private LayerMask _whatIsWall;

    public Transform WallCheckLocation => _wallCheckLocation;

    public float TimeOnWall { get; private set; } = 0;
    public float TimeOffWall { get; private set; } = 0;

    private bool _isAgainstWall = false;
    public bool IsAgainstWall
    {
        get => _isAgainstWall;
        private set
        {
            // if our wall state is about to change
            if (value != _isAgainstWall)
            {
                // if we're about to be on wall and previously weren't
                if (value == true)
                {
                    TimeOffWall = 0;
                    FoundWall?.Invoke();
                }
                // if we're leaving wall and previously were against it
                else if (value == false)
                {
                    TimeOnWall = 0;
                    LostWall?.Invoke();
                }
            }
            _isAgainstWall = value;
        }
    }


    private void FixedUpdate()
    {
        IsAgainstWall = CheckIfAgainstWall();
    }

    private void Update()
    {
        // track Time spent against wall
        if (IsAgainstWall)
            TimeOnWall += Time.deltaTime;
        else
            TimeOffWall += Time.deltaTime;
    }

    public bool CheckIfAgainstWall()
    {
        if (_wallCheckLocation != null)
        {
            return Physics2D.Raycast(_wallCheckLocation.position, transform.right, _wallCheckDistance, _whatIsWall);
        }
        else
        {
            Debug.LogWarning("No groundcheck specified on: " + gameObject.name);
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (_wallCheckLocation != null)
        {
            Gizmos.DrawLine(_wallCheckLocation.position, 
                _wallCheckLocation.position + (transform.right * _wallCheckDistance));
        }
    }
}
