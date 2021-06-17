using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;

    Movement _movement;
    PlayerData _data;
    GameplayInput _input;

    public PlayerMoveState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;

        _movement = player.Actor.Movement;
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

        // if we're moving, 
        if(_input.XInputRaw != 0)
        {
            _movement.MoveX(_data.MoveSpeed * _input.XInputRaw);
        }
        //TODO: otherwise, slow momentum
        else
        {

        }
    }

    public override void Update()
    {
        base.Update();

        if(_input.YInputRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        //TODO: track speed here and only switch if speed is at or near 0, NOT input
        else if(_input.XInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }
}
