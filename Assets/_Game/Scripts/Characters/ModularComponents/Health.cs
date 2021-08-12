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
    private int _max = 10;
    [SerializeField]
    private int _current = 10;
    [SerializeField] 
    private bool _isDamageable = true;

    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    public bool IsDamageable
    {
        get => _isDamageable;
        set => _isDamageable = value;
    }

    public int Current 
    {
        get => _current;
        set
        {
            value = Mathf.Clamp(value, 0, _max);
            if(value != _current)
            {
                HealthChanged?.Invoke(value);
            }
            _current = value;
        }
    }

    public int Max
    {
        get => _max;
        set
        {
            if (value < 1)
                value = 1;
            _max = value;
        }
    }

    public virtual void Heal(int amount)
    {
        Current += amount;
    }

    public virtual void Damage(int amount)
    {
        if (!_isDamageable) return;

        Current -= amount;
        Damaged?.Invoke(amount);

        Current = Mathf.Clamp(Current, 0, _max);

        if(Current == 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Died?.Invoke();
        // extra assurance to make sure we clean up events
        Damaged?.RemoveAllListeners();
        Died?.RemoveAllListeners();

        Destroy(gameObject);
    }
}
