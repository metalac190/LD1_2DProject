using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockbackState : State
{
    private EnemyFSM _stateMachine;

    private ReceiveHit _receiveHit;

    public EnemyKnockbackState(EnemyFSM stateMachine, Enemy enemy)
    {
        _stateMachine = stateMachine;

        _receiveHit = enemy.ReceiveHit;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Enemy Knockback");
        _receiveHit.HitRecovered += OnKnockbackEnded;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveHit.HitRecovered -= OnKnockbackEnded;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnKnockbackEnded()
    {
        _stateMachine.ChangeStateToPrevious();
    }
}
