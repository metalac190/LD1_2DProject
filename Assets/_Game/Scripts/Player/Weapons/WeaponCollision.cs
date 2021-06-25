using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField]
    private WeaponSystem _weaponSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectHitable(collision);
    }

    private void DetectHitable(Collider2D collision)
    {
        ReceiveHit hitable = collision.GetComponent<ReceiveHit>();
        if (hitable != null)
        {
            ApplyHitEffects(collision, hitable);
            // notify weapon system so player behavior can respond
            _weaponSystem.HitOtherObject();
        }
    }

    private void ApplyHitEffects(Collider2D other, ReceiveHit hitable)
    {
        int damage = _weaponSystem.CurrentMeleeAttack.Damage;
        float amount = _weaponSystem.CurrentMeleeAttack.KnockbackAmount;
        float duration = _weaponSystem.CurrentMeleeAttack.KnockbackDuration;
        // add modifier, adjusted for facing direction
        Vector2 direction = CalculateDirection(other);
        Debug.Log("Push Direction: " + direction + " Push Amount: " + amount);
        //pushable.Push(direction, amount, duration);
        HitData hitData = new HitData(other.transform, damage, direction, amount, duration);
        hitable.Hit(hitData);
    }

    private Vector2 CalculateDirection(Collider2D otherCollision)
    {
        Vector2 direction = new Vector2(_weaponSystem.CurrentMeleeAttack.KnockbackForceModifier.x * transform.right.x,
                    _weaponSystem.CurrentMeleeAttack.KnockbackForceModifier.y);
        if (_weaponSystem.CurrentMeleeAttack.AddReverseDirection)
        {
            Vector2 reverseForce = (transform.position - otherCollision.transform.position) * -1;
            // make sure distance isn't creating huge vectors
            reverseForce.Normalize();
            // combine with original force adjustment
            direction += reverseForce;
        }
        // ensure direction can be modified with strength later
        direction.Normalize();
        return direction;
    }
}
