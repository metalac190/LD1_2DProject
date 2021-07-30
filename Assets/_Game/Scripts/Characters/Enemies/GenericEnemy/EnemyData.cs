using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public abstract class EnemyData : ScriptableObject
{
    [Header("Enemy")]
    [SerializeField]
    private string _name = "...";
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private bool _isDamageable = true;

    public string Name => _name;
    public int Health => _health;
    public bool IsDamageable => _isDamageable;
}
