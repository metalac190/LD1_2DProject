using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Based on Tutorial series from Bardent:
/// https://youtu.be/bZVqz_3_NmQ?list=PLy78FINcVmjA0zDBhLuLNL1Jo6xNMMq-W
/// Assets from tutorial deviation
/// https://drive.google.com/drive/folders/1vXTVzzLCr5qVESTxyWLBSru-F2XLeJg4
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private int _amountOfJumps = 1;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = .25f;
    [SerializeField] private float _airMoveForce = 10;
    [SerializeField] private float _airDragMultiplier = 0.95f;
    [SerializeField] private float _variableJumpHeightMultiplier = 0.5f;

    [Header("WallSlide")]
    [SerializeField] private float _wallCheckDistance = 0;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _isWallSlideSpeed = 1;

    [SerializeField] private Vector2 _wallHopDirection = new Vector2(1,0.5f);
    [SerializeField] private Vector2 _wallJumpDirection = new Vector2(1,2);
    [SerializeField] private float _wallHopForce = 1;
    [SerializeField] private float _wallJumpForce = 1;

    private int _remainingJumps = 0;
    private int _facingDirection = 1;

    private bool _isFacingRight = true;
    private bool _isWalking = false;
    private bool _isGrounded = false;
    private bool _canJump = false;
    private bool _isTouchingWall = false;
    private bool _isWallSliding = false;

    private float _movementInputDirection;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if(_isTouchingWall && !_isGrounded && _rb.velocity.y < 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void CheckIfCanJump()
    {
        if((_isGrounded && _rb.velocity.y <= 0) || _isWallSliding)
        {
            _remainingJumps = _amountOfJumps;
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

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 
            _groundCheckRadius, _whatIsGround);
        _isTouchingWall = Physics2D.Raycast
            (_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
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
            Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y 
                * _variableJumpHeightMultiplier);
        }
    }

    private void ApplyMovement()
    {
        // if we're grounded apply normal velocity
        if (_isGrounded)
        {
            _rb.velocity = new Vector2
                (_movementSpeed * _movementInputDirection, _rb.velocity.y);
        }
        // if we're attempting move in the air and not wall sliding, use air force
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(_airMoveForce * _movementInputDirection, 0);
            _rb.AddForce(forceToAdd);

            if (Mathf.Abs(_rb.velocity.x) > _movementSpeed)
            {
                _rb.velocity = new Vector2
                    (_movementSpeed * _movementInputDirection, _rb.velocity.y);
            }
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
        {
            _rb.velocity = new Vector2
                (_rb.velocity.x * _airDragMultiplier, _rb.velocity.y);
        }

        // if we're wall sliding, clamp vertical velocity
        if (_isWallSliding)
        {
            // ensure velocity is capped at wallslide speed
            if(_rb.velocity.y < -_isWallSlideSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_isWallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!_isWallSliding)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void Jump()
    {
        // if this is a non-wallsliding jump
        if (_canJump && !_isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _remainingJumps--;
        }
        // wall hop
        else if(_isWallSliding && _movementInputDirection == 0 && _canJump)
        {
            _isWallSliding = false;
            _amountOfJumps--;
            Vector2 forceToAdd = new Vector2
                (_wallHopForce * _wallHopDirection.x * -_facingDirection, 
                _wallHopForce * _wallHopDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        // wall jump
        else if((_isWallSliding || _isTouchingWall) 
            && _movementInputDirection != 0 && _canJump)
        {
            _isWallSliding = false;
            _amountOfJumps--;
            Vector2 forceToAdd = new Vector2
                (_wallJumpForce * _wallJumpDirection.x * _movementInputDirection,
                _wallJumpForce * _wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.DrawLine(_wallCheck.position, new Vector3
            (_wallCheck.position.x + _wallCheckDistance,
            _wallCheck.position.y, _wallCheck.position.z));
    }
}
