using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
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

            damageable?.Damage(_weaponSystem.EquippedWeapon.DamageAmount);
        }
    }

    private void DamageDetected()
    {
        foreach(IDamageable item in _detectedDamageables)
        {
            Debug.Log("Damage: " + item.ToString());
            item.Damage(_weaponSystem.EquippedWeapon.DamageAmount);
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
}
