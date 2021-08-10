using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;

    MovementKM _movement;
    PlayerMoveData _data;
    GameplayInput _input;
    PlayerAnimator _animator;

    public PlayerMoveState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _animator = player.PlayerAnimator;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Move");

        _animator.PlayAnimation(PlayerAnimator.RunName);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_input.XInputRaw != 0)
        {
            _movement.MoveX(_data.MoveSpeed * _input.XInputRaw, true);
        }
    }

    public override void Update()
    {
        base.Update();

        if (_input.YInputRaw < 0 && _data.AllowCrouch)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        else if(_input.XInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }
}
