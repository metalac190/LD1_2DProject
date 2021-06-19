using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSuperState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    MovementKM _movement;
    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;
    DashSystem _dashSystem;

    public PlayerWallSuperState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _movement = player.Movement;
        _input = player.Input;
        _groundDetector = player.CollisionDetector.GroundDetector;
        _wallDetector = player.CollisionDetector.WallDetector;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;

        _player.ResetJumps();
        _movement.SetGravityScale(0);
    }

    public override void Exit()
    {
        base.Exit();
        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _movement.SetGravityScale(1);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        _wallDetector.DetectWall();

        if (!_wallDetector.IsWallDetected)
        {
            OnLostWall();
        }
    }

    public override void Update()
    {
        base.Update();
        // if we're not holding against the wall, lose it
        if (_input.XInputRaw != _movement.FacingDirection)
        {
            OnLostWall();
        }
    }

    private void OnAttackPressed()
    {
        if (_data.AllowAttack)
        {
            _stateMachine.ChangeState(_stateMachine.WallAttackState);
        }
    }

    private void OnLostWall()
    {
        if (_groundDetector.IsGrounded)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }
    }

    private void OnJumpPressed()
    {
        _stateMachine.ChangeState(_stateMachine.WallJumpState);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.IsReady && _data.AllowDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
