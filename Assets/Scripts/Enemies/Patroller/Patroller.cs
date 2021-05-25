using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PlayerDetector _playerDetector;
    [SerializeField]
    private EnvironmentDetector _environmentDetector;

    [Header("Movement")]
    [SerializeField]
    private float _movementSpeed = 3f;
    [SerializeField]
    private bool _idleOnPathEnd = true;
    [SerializeField]
    private float _minIdleTime = 1;
    [SerializeField]
    private float _maxIdleTime = 2;

    public int FacingDirection { get; private set; } = 1;

    public Rigidbody2D RB => _rb;
    public Animator Animator => _animator;
    public PlayerDetector PlayerDetector => _playerDetector;
    public EnvironmentDetector EnvironmentDetector => _environmentDetector;

    public float MovementSpeed => _movementSpeed;
    public float MinIdleTime => _minIdleTime;
    public float MaxIdleTime => _maxIdleTime;
    public bool IdleOnPathEnd => _idleOnPathEnd;

    private Vector2 _tempVelocity;  // used for temp calculations to avoid creating new Vectors

    public void SetVelocity(float velocity)
    {
        _tempVelocity.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = _tempVelocity;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
}
