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
        DetectDamageable(collision);
        DetectPushable(collision);
    }

    private void DetectDamageable(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            _weaponSystem.Hit(damageable);
        }
    }

    private void DetectPushable(Collider2D collision)
    {
        ReceiveKnockback pushable = collision.GetComponent<ReceiveKnockback>();
        if(pushable != null)
        {
            float amount = _weaponSystem.CurrentMeleeAttack.KnockbackAmount;
            float duration = _weaponSystem.CurrentMeleeAttack.KnockbackDuration;
            // add modifier, adjusted for facing direction
            Vector2 direction = new Vector2(_weaponSystem.CurrentMeleeAttack.KnockbackForceModifier.x * transform.right.x,
                _weaponSystem.CurrentMeleeAttack.KnockbackForceModifier.y);
            if (_weaponSystem.CurrentMeleeAttack.AddReverseDirection)
            {
                Vector2 reverseForce = (transform.position - collision.transform.position) * -1;
                // make sure distance isn't creating huge vectors
                reverseForce.Normalize();
                // combine with original force adjustment
                direction += reverseForce;
            }
            // ensure direction can be modified with strength later
            direction.Normalize();
            Debug.Log("Push Direction: " + direction + " Push Amount: " + amount);
            pushable.Push(direction, amount, duration);
        }
    }
}
