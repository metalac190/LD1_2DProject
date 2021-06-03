using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;

    public PlayerMoveState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Move");

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _player.SetVelocityX(_data.MoveSpeed * _input.XRaw);
    }

    public override void Update()
    {
        base.Update();

        if(_input.YRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        else if(_input.XRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }
}
