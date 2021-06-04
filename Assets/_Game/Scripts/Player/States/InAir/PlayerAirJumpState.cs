using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    PlayerData _data;
    GroundDetector _groundDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;

    public PlayerAirJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.GroundDetector;
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
        _player.SetVelocityY(_data.AirJumpVelocity);

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
        if (!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }

        _player.SetVelocityX(_input.XInputRaw * _data.MoveSpeed);
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
        _player.SetVelocityY(_player.RB.velocity.y * _data.ShortAirJumpHeightScale);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
