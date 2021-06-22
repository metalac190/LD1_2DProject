using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField]
    private WeaponSystem _weaponSystem;
    [SerializeField]
    private float _verticalModifier = 1;

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
        IPushable pushable = collision.GetComponent<IPushable>();
        if(pushable != null)
        {
            float amount = _weaponSystem.CurrentMeleeAttack.KnockbackAmount;
            float duration = _weaponSystem.CurrentMeleeAttack.KnockbackDuration;
            Vector2 direction = (transform.position - collision.transform.position) * -1;
            direction.y += _verticalModifier;
            direction.Normalize();

            Debug.Log("Push: " + direction * amount);
            pushable.Push(direction, amount, duration);
        }
    }
}
