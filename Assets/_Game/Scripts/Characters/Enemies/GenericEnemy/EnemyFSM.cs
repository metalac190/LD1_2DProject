using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : StateMachineMB
{
    [SerializeField]
    private Enemy _enemy;

    public EnemyIdleState IdleState;
    public EnemyKnockbackState KnockbackState;
    public EnemyDeathState DeathState;

    protected virtual void Awake()
    {
        IdleState = new EnemyIdleState(this, _enemy);
        KnockbackState = new EnemyKnockbackState(this, _enemy);
        DeathState = new EnemyDeathState(this, _enemy);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _enemy.Health.Died.AddListener(OnDied);
        _enemy.ReceiveHit.HitReceived.AddListener(OnHitReceived);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _enemy.Health.Died.RemoveListener(OnDied);
        _enemy.ReceiveHit.HitReceived.RemoveListener(OnHitReceived);
    }

    protected virtual void Start()
    {
        ChangeState(IdleState);
    }

    private void OnDied()
    {
        ChangeState(DeathState);
    }

    private void OnHitReceived()
    {
        ChangeState(KnockbackState);
    }
}
