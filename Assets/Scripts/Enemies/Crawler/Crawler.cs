using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    [Header("Environment Detection")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance = 0.2f;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private float _ledgeCheckDistance = 0.4f;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _minIdleTime = 1;
    [SerializeField] private float _maxIdleTime = 2;

    public int FacingDirection { get; private set; } = 1;

    public Rigidbody2D RB => _rb;
    public Animator Animator => _animator;

    public Transform LedgeCheck => _ledgeCheck;
    public float WallCheckDistance => _wallCheckDistance;
    public Transform WallCheck => _wallCheck;
    public float LedgeCheckDistance => _ledgeCheckDistance;
    public LayerMask WhatIsGround => _whatIsGround;

    public float MovementSpeed => _movementSpeed;
    public float MinIdleTime => _minIdleTime;
    public float MaxIdleTime => _maxIdleTime;

    private Vector2 _tempVelocity;  // used for temp calculations to avoid creating new Vectors

    public void SetVelocity(float velocity)
    {
        _tempVelocity.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = _tempVelocity;
        Debug.Log("XVelocity: " + RB.velocity.x);
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(_wallCheck.position, transform.right,
            WallCheckDistance, WhatIsGround);
    }

    public bool CheckLedge()
    {
        // if no ground is detected, it's a ledge
        return !Physics2D.Raycast(_ledgeCheck.position, Vector2.down,
            LedgeCheckDistance, WhatIsGround);
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position +
            (Vector3)(Vector2.right * FacingDirection * WallCheckDistance));
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position +
            (Vector3)(Vector2.down * LedgeCheckDistance));
    }
}
