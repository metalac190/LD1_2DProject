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
    }

    private void DetectDamageable(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            _weaponSystem.Hit(damageable);
        }
    }
}
