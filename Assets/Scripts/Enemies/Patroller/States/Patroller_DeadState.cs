using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_DeadState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PatrollerData _data;

    public Patroller_DeadState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;
    }

    public override void Enter()
    {
        base.Enter();

        _patroller.Remove();
        // instantiate death particles
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
