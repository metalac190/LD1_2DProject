using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;
    DashSystem _dashSystem;

    public PlayerIdleState(PlayerFSM stateMachine, Player player) 
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Idle");
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _groundDetector.LeftGround += OnLeftGround;

        _player.SetVelocityX(0);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _groundDetector.LeftGround -= OnLeftGround;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
    }

    public override void Update()
    {
        base.Update();

        if(_input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }

    private void OnJumpPressed()
    {
        if (_data.AllowJump)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    private void OnDashPressed()
    {
        if (_dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    private void OnLeftGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
