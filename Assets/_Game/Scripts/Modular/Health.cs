using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour, IDamageable
{
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    [Header("Health")]
    [SerializeField] private int _healthMax = 50;
    [SerializeField] private bool _isDamageable = true;

    public bool IsDamageable
    {
        get => _isDamageable;
        set => _isDamageable = value;
    }

    private int _healthCurrent;
    public int HealthCurrent 
    {
        get => _healthCurrent;
        set
        {
            value = Mathf.Clamp(value, 0, _healthMax);
            _healthCurrent = value;
        }
    }

    public int HealthMax
    {
        get => _healthMax;
        set
        {
            if (value < 1)
                value = 1;
            _healthMax = value;
        }
    }

    private void Awake()
    {
        HealthCurrent = _healthMax;
    }

    public virtual void Damage(int amount)
    {
        if (!_isDamageable) return;

        Debug.Log("Damage: " + gameObject.name + " " + amount);
        HealthCurrent -= amount;
        Damaged?.Invoke(amount);
        //TODO Hit Particles

        HealthCurrent = Mathf.Clamp(HealthCurrent, 0, _healthMax);
        if(HealthCurrent == 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Died?.Invoke();
    }
}
