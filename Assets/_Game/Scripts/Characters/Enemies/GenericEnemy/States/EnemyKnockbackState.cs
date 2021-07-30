using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockbackState : State
{
    private EnemyFSM _stateMachine;
    private Enemy _enemy;

    private ReceiveKnockback _receiveKnockback;

    public EnemyKnockbackState(EnemyFSM stateMachine, Enemy enemy)
    {
        _stateMachine = stateMachine;
        _enemy = enemy;

        _receiveKnockback = enemy.ReceiveKnockback;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Enemy Knockback");
        _receiveKnockback.KnockbackEnded += OnKnockbackEnded;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveKnockback.KnockbackEnded -= OnKnockbackEnded;
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
