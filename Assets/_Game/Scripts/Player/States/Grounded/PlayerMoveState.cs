using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;

    public PlayerMoveState(PlayerFSM stateMachine, Player player) 
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Move");
        
        _input.JumpPressed += OnJumpPressed;
        _groundDetector.LeftGround += OnLeftGround;
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _groundDetector.LeftGround -= OnLeftGround;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();

        _player.SetVelocityX(_data.MoveSpeed * _input.XRaw);
    }

    public override void Update()
    {
        base.Update();

        if(_input.XRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void OnJumpPressed()
    {
        //if (_player.JumpsRemaining <= 0) { return; }
        if (_data.AllowJump)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    private void OnLeftGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
