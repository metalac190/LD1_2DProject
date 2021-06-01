using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : AbilitySuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    public PlayerDashState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;
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