using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroState : State
{
    private LevelFSM _stateMachine;

    public LevelIntroState(LevelFSM stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        // spawn player if a player doesn't already exist
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
