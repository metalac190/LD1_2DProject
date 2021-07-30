using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<int> HealthChanged;

    [Header("Health")]
    [SerializeField]
    private int _healthMax = 10;
    [SerializeField] 
    private bool _isDamageable = true;

    public UnityEvent<int> Damaged;
    public UnityEvent Died;

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
            if(value != _healthCurrent)
            {
                HealthChanged?.Invoke(value);
            }
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

        HealthCurrent -= amount;
        Damaged?.Invoke(amount);

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
