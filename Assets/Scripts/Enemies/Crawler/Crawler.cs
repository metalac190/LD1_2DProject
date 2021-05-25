using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] 
    private Rigidbody2D _rb;
    [SerializeField] 
    private Animator _animator;
    [SerializeField]
    private EnvironmentDetector _environmentChecker;

    [Header("Movement")]
    [SerializeField] 
    private float _movementSpeed = 3f;

    public int FacingDirection { get; private set; } = 1;

    public Rigidbody2D RB => _rb;
    public Animator Animator => _animator;
    public EnvironmentDetector EnvironmentChecker => _environmentChecker;

    public float MovementSpeed => _movementSpeed;

    private Vector2 _tempVelocity;  // used for temp calculations to avoid creating new Vectors

    public void SetVelocity(float velocity)
    {
        _tempVelocity.Set(FacingDirection * velocity, RB.velocity.y);
        RB.velocity = _tempVelocity;
        Debug.Log("XVelocity: " + RB.velocity.x);
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }
}
