using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Based on Tutorial series from Bardent:
/// https://youtu.be/bZVqz_3_NmQ?list=PLy78FINcVmjA0zDBhLuLNL1Jo6xNMMq-W
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private int _amountOfJumps = 1;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = .25f;
    [SerializeField] private float _airMoveForce = 10;
    [SerializeField] private float _airDragMultiplier = 0.95f;
    [SerializeField] private float _variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private float _jumpTimerSet = 0.15f;

    [Header("Wall Slide")]
    [Tooltip("Distance from wallCheck object to cast wall check ray")]
    [SerializeField] private float _wallCheckDistance = .65f;
    [Tooltip("Object to start casting wall check raycast")]
    [SerializeField] private Transform _wallCheck;
    [Tooltip("Downwards velocity while wall sliding")]
    [SerializeField] private float _wallSlideSpeed = 1;
    [Tooltip("Duration allowed for switching direction while wall jumping")]
    [SerializeField] private float _turnTimerSet = 0.2f;
    [SerializeField] private float _wallJumpTimerSet = 0.5f;
    [SerializeField] private Vector2 _wallHopDirection = new Vector2(1,0.5f);
    [SerializeField] private Vector2 _wallJumpDirection = new Vector2(1,2);
    [SerializeField] private float _wallHopForce = 1;
    [SerializeField] private float _wallJumpForce = 1;

    [Header("Ledge Grab")]
    [SerializeField] Transform _ledgeCheck;
    [SerializeField] float _ledgeClimbXOffset1 = 0.3f;
    [SerializeField] float _ledgeClimbYOffset1 = 0;
    [SerializeField] float _ledgeClimbXOffset2 = .5f;
    [SerializeField] float _ledgeClimbYOffset2 = 2;
    [SerializeField] float _ledgeClimbDuration = .5f;

    private int _remainingJumps = 0;
    private int _facingDirection = 1;
    private int _lastWallJumpDirection = 0;

    private float _movementInputDirection;
    private float _jumpTimer = 0;
    private float _turnTimer = 0;
    private float _wallJumpTimer = 0;

    private bool _isFacingRight = true;
    private bool _isWalking = false;
    private bool _isGrounded = false;
    private bool _canJump = false;
    private bool _canWallJump = false;
    private bool _isTouchingWall = false;
    private bool _isWallSliding = false;
    private bool _isAttemptingJump = false;   // if player has requested jump that hasn't happened yet
    private bool _checkJumpMultiplier = false;
    private bool _canMove = false;
    private bool _canFlip = false;
    private bool _hasWallJumped = false;
    private bool _isTouchingLedge = false;
    private bool _canClimbLedge = false;
    private bool _ledgeDetected = false;

    private Vector2 _ledgePosBottom;
    private Vector2 _ledgePosStart;
    private Vector2 _ledgePosTarget;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Coroutine _ledgeClimbRoutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        // initialize
        _remainingJumps = _amountOfJumps;
        _wallHopDirection.Normalize();
        _wallJumpDirection.Normalize();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        // if we're touching a wall and not near a ledge, wall slide
        if(_isTouchingWall && _movementInputDirection == _facingDirection && _rb.velocity.y < 0
            && !_canClimbLedge)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void CheckLedgeClimb()
    {
        // detect the first time we can climb a ledge and flag the climb state change
        if(_ledgeDetected && !_canClimbLedge)
        {
            _canClimbLedge = true;
            // calculate ledge positions
            if (_isFacingRight)
            {
                _ledgePosStart = new Vector2(Mathf.Floor(_ledgePosBottom.x + _wallCheckDistance) - _ledgeClimbXOffset1,
                    Mathf.Floor(_ledgePosBottom.y) + _ledgeClimbYOffset1);
                _ledgePosTarget = new Vector2(Mathf.Floor(_ledgePosBottom.x + _wallCheckDistance) + _ledgeClimbXOffset2,
                    Mathf.Floor(_ledgePosBottom.y) + _ledgeClimbYOffset2);
            }
            // if it's on the left, reverse the calculation
            else
            {
                _ledgePosStart = new Vector2(Mathf.Ceil(_ledgePosBottom.x - _wallCheckDistance) + _ledgeClimbXOffset1,
                    Mathf.Floor(_ledgePosBottom.y) + _ledgeClimbYOffset1);
                _ledgePosTarget = new Vector2(Mathf.Ceil(_ledgePosBottom.x - _wallCheckDistance) - _ledgeClimbXOffset2,
                    Mathf.Floor(_ledgePosBottom.y) + _ledgeClimbYOffset2);
            }
            // remove control of the character while we're climbing
            _canMove = false;
            _canFlip = false;
            // tell animator
            _animator.SetBool("CanClimbLedge", _canClimbLedge);
            _ledgeClimbRoutine = StartCoroutine(LedgeClimbRoutine(_ledgeClimbDuration));
        }

        // if climbing, hold position
        if (_canClimbLedge)
        {
            _rb.position = _ledgePosStart;
        }
    }

    private void CheckIfCanJump()
    {
        if(_isGrounded && _rb.velocity.y <= 0.01f)
        {
            _remainingJumps = _amountOfJumps;
        }

        if (_isTouchingWall)
        {
            _canWallJump = true;
        }

        if(_remainingJumps <= 0)
        {
            _canJump = false;
        }
        else
        {
            _canJump = true;
        }
    }

    public void FinishLedgeClimb()
    {
        _canClimbLedge = false;
        _rb.position = _ledgePosTarget;
        // return control to the player
        _canMove = true;
        _canFlip = true;
        // allow new ledges to be detected
        _ledgeDetected = false;
        // tell animator
        _animator.SetBool("CanClimbLedge", _canClimbLedge);
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 
            _groundCheckRadius, _whatIsGround);
        _isTouchingWall = Physics2D.Raycast
            (_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
        _isTouchingLedge = Physics2D.Raycast
            (_ledgeCheck.position, transform.right, _wallCheckDistance, _whatIsGround);

        // only detect ledge once
        if(_isTouchingWall && !_isTouchingLedge && !_ledgeDetected)
        {
            _ledgeDetected = true;
            _ledgePosBottom = _wallCheck.position;
        }
    }

    private void CheckMovementDirection()
    {
        // determine which direction to face
        if(_isFacingRight && _movementInputDirection < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && _movementInputDirection > 0)
        {
            Flip();
        }
        // determine if we're walking
        if(_rb.velocity.x != 0)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("IsWalking", _isWalking);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("YVelocity", _rb.velocity.y);
        _animator.SetBool("IsWallSliding", _isWallSliding);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if(_isGrounded || (_remainingJumps > 0 && _isTouchingWall))
            {
                Jump();
            }
            else
            {
                _jumpTimer = _jumpTimerSet;
                _isAttemptingJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && _isTouchingWall)
        {
            // if we're briefly attempting to change direction away from a wall
            if(!_isGrounded && _movementInputDirection != _facingDirection)
            {
                _canMove = false;
                _canFlip = false;
                // lock temporarily with timer
                _turnTimer = _turnTimerSet;
            }
        }
        // lock movement until our turn timer expires
        if (_turnTimer >= 0)
        {
            _turnTimer -= Time.deltaTime;
            if(_turnTimer <= 0)
            {
                _canMove = true;
                _canFlip = true;
            }
        }

        if (_checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            _checkJumpMultiplier = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y 
                * _variableJumpHeightMultiplier);
        }
    }

    private void ApplyMovement()
    {
        // we're freefalling and there's not input
        if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
        {
            _rb.velocity = new Vector2
                (_rb.velocity.x * _airDragMultiplier, _rb.velocity.y);
        }
        // if we're grounded apply normal velocity from input
        else if(_canMove)
        {
            _rb.velocity = new Vector2
                (_movementSpeed * _movementInputDirection, _rb.velocity.y);
        }

        // if we're wall sliding, clamp vertical velocity
        if (_isWallSliding)
        {
            // ensure velocity is capped at wallslide speed
            if(_rb.velocity.y < -_wallSlideSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!_isWallSliding && _canFlip)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void CheckJump()
    {
        if(_jumpTimer > 0)
        {
            // wall jump
            if(!_isGrounded && _isTouchingWall && _movementInputDirection != 0 
                && _movementInputDirection != _facingDirection)
            {
                WallJump();
            }
            else if (_isGrounded)
            {
                Jump();
            }
        }

        if(_isAttemptingJump)
        {
            _jumpTimer -= Time.deltaTime;
        }

        if(_wallJumpTimer > 0)
        {
            if(_hasWallJumped && _movementInputDirection == -_lastWallJumpDirection)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _hasWallJumped = false;
            }
            else if(_wallJumpTimer <= 0)
            {
                _hasWallJumped = false;
            }
            else
            {
                _wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (_canJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _remainingJumps--;
            _jumpTimer = 0;
            _isAttemptingJump = false;
            _checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (_canWallJump)
        {
            // kill current y velocity
            _rb.velocity = new Vector2(_rb.velocity.x, 0);  
            _isWallSliding = false;
            // wall jump refills 'grounded' extra jumps
            _remainingJumps = _amountOfJumps;
            _remainingJumps--;
            // apply upwards force
            Vector2 forceToAdd = new Vector2
                (_wallJumpForce * _wallJumpDirection.x * _movementInputDirection,
                _wallJumpForce * _wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            _jumpTimer = 0;
            _isAttemptingJump = false;
            _checkJumpMultiplier = true;
            // prevent character from sticking to the wall
            _turnTimer = 0;
            _canMove = true;
            _canFlip = true;
            _hasWallJumped = true;
            // prevent single hopping the same wall
            _wallJumpTimer = _wallJumpTimerSet;
            _lastWallJumpDirection = -_facingDirection;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.DrawLine(_wallCheck.position, new Vector3
            (_wallCheck.position.x + _wallCheckDistance,
            _wallCheck.position.y, _wallCheck.position.z));
        Gizmos.DrawLine(_ledgeCheck.position, new Vector3
            (_ledgeCheck.position.x + _wallCheckDistance,
            _ledgeCheck.position.y, _ledgeCheck.position.z));
        Gizmos.DrawLine(_ledgePosStart, _ledgePosTarget);
    }

    IEnumerator LedgeClimbRoutine(float duration)
    {
        _collider.enabled = false;
        // animate from start to end position
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            _rb.position = Vector2.Lerp(_ledgePosStart, _ledgePosTarget, elapsedTime / duration);
            yield return null;
        }
        _rb.position = _ledgePosTarget;
        _collider.enabled = true;

        FinishLedgeClimb();
    }
}
