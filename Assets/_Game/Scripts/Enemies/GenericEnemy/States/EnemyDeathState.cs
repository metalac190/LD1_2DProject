using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : State
{
    private EnemyFSM _stateMachine;
    private EnemyData _data;

    public EnemyDeathState(EnemyFSM stateMachine, Enemy enemy)
    {
        _stateMachine = stateMachine;
        _data = enemy.EnemyData;
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

        if(StateDuration >= _data.DeathDuration)
        {
            Object.Destroy(_stateMachine.gameObject);
        }
    }
}
