using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Base")]
    [SerializeField]
    private EnemyData _enemyData;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private ReceiveHit _receiveHit;

    public EnemyData EnemyData => _enemyData;
    public Health Health => _health;
    public ReceiveHit ReceiveHit => _receiveHit;

    protected virtual void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _health.Max = _enemyData.Health;
        _health.Current = _enemyData.Health;
        _health.IsDamageable = _enemyData.IsDamageable;
    }
}
