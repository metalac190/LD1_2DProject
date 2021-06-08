using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCollision : MonoBehaviour
{
    public event Action<IDamageable> HitDamageable;

    private List<IDamageable> _detectedDamageables = new List<IDamageable>();
    [SerializeField]
    private WeaponSystem _weaponSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //AddToDetected(collision);
        Damage(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       //RemoveFromDetected(collision);
    }

    private void Damage(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.Damage(_weaponSystem.CurrentMeleeAttack.Damage);
            HitDamageable?.Invoke(damageable);
        }
    }

    /*
    private void DamageDetected()
    {
        foreach(IDamageable item in _detectedDamageables)
        {
            Debug.Log("Damage: " + item.ToString());
            item.Damage(_weaponSystem.EquippedWeapon.GroundHitDamage);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            _detectedDamageables.Add(damageable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            _detectedDamageables.Add(damageable);
        }
    }
    */
}
