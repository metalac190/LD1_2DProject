using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EnvironmentDetector _environmentDetector;

    public Rigidbody2D RB => _rb;
    public EnvironmentDetector EnvironmentDetector => _environmentDetector;

    public int FacingDirection { get; private set; } = 1;

    private Vector2 _tempVelocity;  // used for temp calculations to avoid creating new Vectors

    public virtual void Push(Vector2 direction, float amount)
    {
        direction.Normalize();
        _tempVelocity.Set(direction.x * amount, direction.y * amount);
        _rb.velocity = _tempVelocity;
    }

    public virtual void Move(float velocity)
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
