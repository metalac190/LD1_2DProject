using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiveState : State
{
    private LevelFSM _stateMachine;

    public LevelActiveState(LevelFSM stateMachine)
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
