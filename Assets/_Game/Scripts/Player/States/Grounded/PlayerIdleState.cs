using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    KinematicObject _movement;
    GameplayInput _input;

    public PlayerIdleState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Movement;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Idle");

        //_movement.SetVelocityXZero();
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

        if (_input.XInputRaw != 0 && _input.YInputRaw >= 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }

        else if (_input.XInputRaw != 0 && _input.YInputRaw < 0)
        {
            
        }
        else if (_input.XInputRaw == 0 && _input.YInputRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }

        base.Update();
    }
}
