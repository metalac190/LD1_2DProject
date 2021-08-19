using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardProjectile : ProjectileBase
{
    [Header("Forward Projectile")]
    [SerializeField]
    private float _moveSpeed = 5;

    public override void Move(MovementKM movement)
    {
        movement.MoveX(_moveSpeed * movement.FacingDirection, true);
        Vector3 newPosition = transform.right * _moveSpeed * Time.deltaTime;
        transform.position += newPosition;
    }
}
