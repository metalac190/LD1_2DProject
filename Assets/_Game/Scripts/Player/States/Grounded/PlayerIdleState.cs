using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;
    PlayerData _data;

    KinematicObject _movement;
    GameplayInput _input;

    public PlayerIdleState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;
        _data = player.Data;

        _movement = player.Movement;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();

        _animator.PlayAnimation(PlayerAnimator.IdleName);
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
        _movement.CheckIfShouldFlip(_input.XInputRaw);

        if (_input.XInputRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }

        else if (_input.XInputRaw == 0 && _input.YInputRaw < 0
            && _data.AllowCrouch)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }

        base.Update();
    }
}
