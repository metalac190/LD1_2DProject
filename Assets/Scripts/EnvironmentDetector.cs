using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    [Header("Environment Detection")]
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private float _wallCheckDistance = 0.2f;
    [SerializeField]
    private Transform _ledgeCheck;
    [SerializeField]
    private float _ledgeCheckDistance = 0.4f;
    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private bool _startCheckingOnAwake = true;

    private bool _isCheckingEnvironment = false;
    private bool _isWallDetected = false;
    private bool _isLedgeDetected = false;

    public bool IsWallDetected => _isWallDetected;
    public bool IsLedgeDetected => _isLedgeDetected;

    #region Public Methods

    private void Start()
    {
        if (_startCheckingOnAwake)
        {
            StartCheckingEnvironment();
        }
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(_wallCheck.position, transform.right,
            _wallCheckDistance, _whatIsGround);
    }

    public bool CheckLedge()
    {
        bool isGroundDetected = Physics2D.Raycast(_ledgeCheck.position, Vector2.down,
            _ledgeCheckDistance, _whatIsGround);
        // if there's no ground detected, we've hit a ledge
        return !isGroundDetected;
    }

    private void FixedUpdate()
    {
        // check in fixed update to ensure ledges/walls are not skipped
        if (_isCheckingEnvironment)
        {
            _isWallDetected = CheckWall();
            _isLedgeDetected = CheckLedge();
        }
    }

    public void StartCheckingEnvironment()
    {
        _isCheckingEnvironment = true;
    }

    public void StopCheckingEnvironment()
    {
        _isCheckingEnvironment = false;
    }

    #endregion

    #region Private Methods

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position
            + (transform.right * _wallCheckDistance));
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position
            + ((transform.up*-1) * _ledgeCheckDistance));
    }
    #endregion
}
