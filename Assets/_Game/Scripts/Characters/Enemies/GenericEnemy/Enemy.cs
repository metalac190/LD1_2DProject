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
    [SerializeField]
    private ReceiveKnockback _receiveKnockback;

    public EnemyData EnemyData => _enemyData;
    public Health Health => _health;
    public ReceiveHit ReceiveHit => _receiveHit;
    public ReceiveKnockback ReceiveKnockback => _receiveKnockback;

    protected virtual void Awake()
    {
        _health.HealthMax = _enemyData.Health;
        _health.IsDamageable = _enemyData.IsDamageable;
    }
}
