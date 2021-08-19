using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAggroState : State
{
    private ShooterFSM _stateMachine;

    public ShooterAggroState(ShooterFSM stateMachine, Shooter shooter)
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
