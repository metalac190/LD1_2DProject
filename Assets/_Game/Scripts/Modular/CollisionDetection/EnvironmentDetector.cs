using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    [Header("Environment Detection")]
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundCheckRadius = 0.3f;
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

    private bool _isGroundDetected = false;
    private bool _isWallDetected = false;
    private bool _isLedgeDetected = false;

    public bool IsGrounded => _isGroundDetected;
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
    
    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(_wallCheck.position, transform.right,
            _wallCheckDistance, _whatIsGround);
    }

    public bool CheckLedge()
    {
        // only check for ledge if we know we're grounded
        if (_isGroundDetected)
        {
            bool isGroundAtLedge = Physics2D.Raycast(_ledgeCheck.position, Vector2.down,
                _ledgeCheckDistance, _whatIsGround);
            // if we DONT find ground at the ledge, then it's a ledge
            if (!isGroundAtLedge)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        // check in fixed update to ensure ledges/walls are not skipped
        if (_isCheckingEnvironment)
        {
            _isGroundDetected = CheckGround();
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
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);

        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position
            + (transform.right * _wallCheckDistance));

        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position
            + ((transform.up*-1) * _ledgeCheckDistance));
    }
    #endregion
}
