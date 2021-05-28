using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    [Header("Health")]
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private bool _isDamageable = true;

    public bool IsDamageable
    {
        get => _isDamageable;
        set
        {
            _isDamageable = value;
        }
    }

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (!_isDamageable) return;

        Debug.Log("Damaged");
        _currentHealth -= amount;
        Damaged?.Invoke(amount);
        //TODO Hit Particles

        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        if(_currentHealth == 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Died?.Invoke();
    }
}
