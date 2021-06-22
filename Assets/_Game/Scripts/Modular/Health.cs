using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using SoundSystem;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<int> HealthChanged;

    [Header("Health")]
    [SerializeField]
    private int _healthMax = 10;
    [SerializeField] 
    private bool _isDamageable = true;
    [SerializeField]
    private Color _flashColor = Color.white;
    [SerializeField] 
    private SpriteRenderer _renderer;
    [SerializeField]
    private SFXOneShot _damagedSFX;

    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private DamageFlash _damageFlash;

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
        if (_renderer != null)
            _damageFlash = new DamageFlash(this, _renderer, _flashColor);
        else
            Debug.LogError("No renderer assigned to Health component");

        HealthCurrent = _healthMax;
    }

    void OnDisable()
    {
        _damageFlash?.StopFlash();
    }

    public virtual void Damage(int amount)
    {
        if (!_isDamageable) return;

        Debug.Log("Damage: " + gameObject.name + " " + amount);
        HealthCurrent -= amount;
        Damaged?.Invoke(amount);

        _damagedSFX?.PlayOneShot(transform.position);
        _damageFlash?.Flash();
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
