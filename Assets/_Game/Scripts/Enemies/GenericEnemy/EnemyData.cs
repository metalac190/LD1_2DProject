using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("Enemy")]
    [SerializeField]
    private string _name = "...";
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private bool _isDamageable = true;
    [SerializeField]
    private bool _isHittable = true;
    [SerializeField]
    private float _deathDuration = 0;

    public string Name => _name;
    public int Health => _health;
    public bool IsDamageable => _isDamageable;
    public bool IsHittable => _isHittable;
    public float DeathDuration => _deathDuration;
}
