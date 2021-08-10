using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : State
{
    private EnemyFSM _stateMachine;
    private Enemy _enemy;

    public EnemyDeathState(EnemyFSM stateMachine, Enemy enemy)
    {
        _stateMachine = stateMachine;
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //TODO add death animation here
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        Object.Destroy(_stateMachine.gameObject);
    }
}
