using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Based on Muddy Wolf Gmaes 2D Platformer tutorial. Link:
/// https://www.youtube.com/watch?v=1bHVsxw_o7o&list=PLfX6C2dxVyLw5kerGvTxB-8xqVINe85gw&index=3&ab_channel=MuddyWolfGames
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _jumpForce = 20f;
    [SerializeField] Rigidbody2D _rb;

    [Header("Ground Detection")]
    [SerializeField] Transform _feetLocation;
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _detectRadius = 0.5f;

    bool _isRunning = false;
    bool _isGrounded = false;
    float _moveX;

    public bool IsRunning => _isRunning;
    public bool IsGrounded => _isGrounded;
    public float MoveX => _moveX;

    void Update()
    {
        DetectMoveInput();
        DetermineIfRunning();
        CheckForTransformFlip();

        CheckForGrounded();
        DetectJumpInput();
    }

    private void DetectMoveInput()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
    }

    private void DetectJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // and we're grounded...
            if (_isGrounded)
            {
                Jump();
            }
        }
    }

    private void DetermineIfRunning()
    {
        _isRunning = (Mathf.Abs(_moveX) > 0.05) ? true : false;
    }

    public void Jump()
    {
        Vector2 newMovement = new Vector2(_rb.velocity.x, _jumpForce);
        _rb.velocity = newMovement;
    }

    private void FixedUpdate()
    {
        Vector2 newMovement = new Vector2(_moveX * _moveSpeed, _rb.velocity.y);
        _rb.velocity = newMovement;
    }

    void CheckForGrounded()
    {
        // test for collider overlap at feet
        Collider2D groundCheck = Physics2D.OverlapCircle
            (_feetLocation.position, _detectRadius, _groundLayers);

        _isGrounded = groundCheck != null;
    }

    void CheckForTransformFlip()
    {
        // if moving right, face character to the right
        if(_moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // if moving left, face character to the left
        else if(_moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
