using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterIdleState : State
{
    private ShooterFSM _stateMachine;

    public ShooterIdleState(ShooterFSM stateMachine, Shooter shooter)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
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
    }
}
