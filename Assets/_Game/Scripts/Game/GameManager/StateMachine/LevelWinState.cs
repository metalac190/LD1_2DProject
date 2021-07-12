using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWinState : State
{
    private LevelFSM _stateMachine;

    public LevelWinState(LevelFSM stateMachine)
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
