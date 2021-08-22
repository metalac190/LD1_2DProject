using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    MovementKM _movement;
    GameplayInput _input;
    PlayerMoveData _data;
    OverlapDetector _groundDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;
    ParticleSystem _jumpDust;

    public PlayerAirJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.EnvironmentDetector.GroundDetector;
        _dashSystem = player.DashSystem;
        _sfx = player.SFX;
        _jumpDust = player.Visuals.JumpDust;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Air Jump");
        _animator.PlayAnimation(PlayerAnimator.JumpName);

        _input.AttackPressed += OnAttackPressed;
        _input.JumpReleased += OnJumpReleased;
        _input.DashPressed += OnDashPressed;

        _player.DecreaseAirJumpsRemaining();

        _movement.SetVelocityYZero();
        _movement.MoveY(_data.AirJumpVelocity);

        _sfx.AirJumpSFX?.PlayOneShot(_player.transform.position);
        _jumpDust?.Play();
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

        _groundDetector.Detect();
        // if we're not grounded, but began falling, go to fall state
        if (_groundDetector.IsDetected == false && _movement.Velocity.y < 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }

        _movement.MoveX(_input.XInputRaw * _data.MoveSpeed, true);
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnAttackPressed()
    {
        if (_input.YInputRaw == -1)
        {
            _movement.MoveY(_movement.Velocity.y * _data.AirAttackVelocityYDampen);
            _stateMachine.ChangeState(_stateMachine.BounceAttackState);
            return;
        }
        else
        {
            _movement.MoveY(_movement.Velocity.y * _data.AirAttackVelocityYDampen);
            _stateMachine.ChangeState(_stateMachine.AirAttackState);
            return;
        }
    }

    private void OnJumpReleased()
    {
        // cut the jump short on release
        _movement.MoveY(_movement.Velocity.y * _data.ShortAirJumpHeightScale);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.IsReady)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
