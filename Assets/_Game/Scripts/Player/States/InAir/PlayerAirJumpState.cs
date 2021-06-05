using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    GameplayInput _input;
    PlayerData _data;
    GroundDetector _groundDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;

    public PlayerAirJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.Actor.CollisionDetector.GroundDetector;
        _dashSystem = player.DashSystem;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();

        _input.AttackPressed += OnAttackPressed;
        _input.JumpReleased += OnJumpReleased;
        _input.DashPressed += OnDashPressed;

        _player.DecreaseAirJumpsRemaining();
        _movement.SetVelocityY(_data.AirJumpVelocity);

        _sfx.AirJumpSFX?.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        _input.AttackPressed -= OnAttackPressed;
        _input.JumpReleased -= OnJumpReleased;
        _input.DashPressed -= OnDashPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        // if we're not grounded, but began falling, go to fall state
        if (!_groundDetector.IsGrounded && _movement.Velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }

        _movement.SetVelocityX(_input.XInputRaw * _data.MoveSpeed);
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnAttackPressed()
    {
        //_stateMachine.ChangeState(_stateMachine.AttackState);
    }

    private void OnJumpReleased()
    {
        // cut the jump short on release
        _movement.SetVelocityY(_movement.Velocity.y * _data.ShortAirJumpHeightScale);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
