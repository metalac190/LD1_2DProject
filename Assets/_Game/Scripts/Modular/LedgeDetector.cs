using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// For proper ledge detection we need to utilize both wall detecting and ground detecting.
/// This script allows you to read whether or not there is a ledge above us and/or a ledge below us.
/// </summary>
public class LedgeDetector : MonoBehaviour
{
    public event Action FoundUpperLedge;
    public event Action LostUpperLedge;

    public event Action FoundLowerLedge;
    public event Action LostLowerLedge;

    [SerializeField]
    private WallDetector _wallDetector;
    [SerializeField]
    private GroundDetector _groundDetector;

    [Header("Ledge detection")]
    [SerializeField]
    private Transform _upperLedgeCheckLocation;
    [SerializeField]
    private float _upperLedgeCheckDistance = 0.5f;
    [SerializeField]
    private Transform _lowerLedgeCheckLocation;
    [SerializeField]
    private float _lowerLedgeCheckDistance = 0.5f;
    [SerializeField]
    private LayerMask _whatIsWall;
    [SerializeField]
    private LayerMask _whatIsGround;

    private bool _isDetectingUpperLedge = false;
    public bool IsDetectingUpperLedge
    {
        get 
        {
            // never return true if we're paused
            if (IsDetectPaused)
                return false;
            else
                return _isDetectingUpperLedge;
        }
        private set
        {
            // if our wall state is about to change
            if (value != _isDetectingUpperLedge)
            {
                // if we're about to be on wall and previously weren't
                if (value == true)
                {
                    FoundUpperLedge?.Invoke();
                }
                // if we're leaving wall and previously were against it
                else if (value == false)
                {
                    LostUpperLedge?.Invoke();
                }
            }

            _isDetectingUpperLedge = value;
        }
    }

    private bool _isDetectingLowerLedge = false;
    public bool IsDetectingLowerLedge
    {
        get
        {
            if (IsDetectPaused)
                return false;
            else
                return _isDetectingLowerLedge;
        }
        private set
        {
            // if our wall state is about to change
            if (value != _isDetectingLowerLedge)
            {
                // if we're about to be on wall and previously weren't
                if (value == true)
                {
                    FoundUpperLedge?.Invoke();
                }
                // if we're leaving wall and previously were against it
                else if (value == false)
                {
                    LostUpperLedge?.Invoke();
                }
            }
            _isDetectingLowerLedge = value;
        }
    }

    public bool IsDetectPaused { get; private set; } = false;
    Coroutine _pauseRoutine;

    public bool DetectUpperLedge()
    {
        // if we're paused, DONT detect
        if(IsDetectPaused) 
        {
            IsDetectingUpperLedge = false;
            return false; 
        }

        if (_upperLedgeCheckLocation != null)
        {
            bool wallPresentNearTop = Physics2D.Raycast(_upperLedgeCheckLocation.position,
                transform.right, _upperLedgeCheckDistance, _whatIsWall);
            // if we're against a wall, but there's nothing above it, we're beneath a ledge
            if (_wallDetector.IsWallDetected && !wallPresentNearTop)
            {
                IsDetectingUpperLedge = true;
                return IsDetectingUpperLedge;
            }

            else
            {
                IsDetectingUpperLedge = false;
                return IsDetectingUpperLedge;
            }
                
        }
        else
        {
            Debug.LogWarning("No upper ledge check specified on: " + gameObject.name);
            return false;
        }
    }

    public bool CheckLowerLedge()
    {
        // if we're paused, DONT detect
        if (IsDetectPaused) 
        {
            IsDetectPaused = false;
            return false; 
        }

        if (_lowerLedgeCheckLocation != null && !IsDetectPaused)
        {
            bool groundPresentNearFront = Physics2D.Raycast(_lowerLedgeCheckLocation.position,
                Vector2.down, _lowerLedgeCheckDistance, _whatIsWall);
            // if we're grounded but there's no ground shortly in front of us, it's a lower ledge
            if (_groundDetector.IsGrounded && !groundPresentNearFront)
            {
                IsDetectingLowerLedge = true;
                return IsDetectingLowerLedge;
            }
                
            else
            {
                IsDetectingLowerLedge = false;
                return IsDetectingLowerLedge;
            }
        }
        else
        {
            Debug.LogWarning("No lower ledge check specified on: " + gameObject.name);
            return false;
        }
    }

    // determine the corner position of our upper ledge
    public Vector2 CalculateUpperLedgeCornerPosition(int facingDirection)
    {
        // get distance from wall
        RaycastHit2D xHit = Physics2D.Raycast(_wallDetector.WallCheckLocation.position, transform.right);
        // add a tiny bit of tolerance to make sure we're far enough over ground
        float tolerance = 0.015f;
        float xDistanceFromWall = xHit.distance + tolerance;
        // cast downwards from top to get distance to ground
        RaycastHit2D yHit = Physics2D.Raycast(_upperLedgeCheckLocation.position
            + new Vector3(xDistanceFromWall * facingDirection, 0, 0),
            Vector2.down, 
            _upperLedgeCheckLocation.position.y - (_wallDetector.WallCheckLocation.position.y + tolerance),
            _whatIsGround);
        float yDistanceFromGround = yHit.distance;
        // combine distances to calculate the corner (wall + corner height)
        Vector2 cornerPosition = new Vector2(_wallDetector.WallCheckLocation.position.x
            + (xDistanceFromWall * facingDirection),
            _upperLedgeCheckLocation.position.y - yDistanceFromGround);

        return cornerPosition;
    }

    public void Pause(float duration)
    {
        if (_pauseRoutine != null)
            StopCoroutine(_pauseRoutine);
        _pauseRoutine = StartCoroutine(PauseRoutine(duration));
    }

    private IEnumerator PauseRoutine(float duration)
    {
        IsDetectPaused = true;
        yield return new WaitForSeconds(duration);
        IsDetectPaused = false;
    }

    private void OnDrawGizmos()
    {
        if (_upperLedgeCheckLocation != null)
        {
            Gizmos.DrawLine(_upperLedgeCheckLocation.position,
                _upperLedgeCheckLocation.position + (transform.right * _upperLedgeCheckDistance));
        }
        if(_lowerLedgeCheckLocation != null)
        {
            Gizmos.DrawLine(_lowerLedgeCheckLocation.position,
                _lowerLedgeCheckLocation.position + (Vector3.down * _lowerLedgeCheckDistance));
        }
    }
}
