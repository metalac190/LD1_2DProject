using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    KinematicObject _movement;
    GameplayInput _input;
    PlayerData _data;
    GroundDetector _groundDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;

    public PlayerJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.CollisionDetector.GroundDetector;
        _dashSystem = player.DashSystem;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Jump");
        _animator.PlayAnimation(PlayerAnimator.JumpName);

        _input.JumpPressed += OnJumpPressed;
        _input.JumpReleased += OnJumpReleased;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;

        _movement.MoveY(_data.JumpVelocity);

        _sfx.JumpSFX?.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.JumpReleased -= OnJumpReleased;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        // if we're not grounded, but began falling, go to fall state
        if(!_groundDetector.IsGrounded && _movement.Velocity.y <= 0)
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
        _movement.MoveY(_movement.Velocity.y * _data.AirAttackVelocityYDampen);
        _stateMachine.ChangeState(_stateMachine.AirAttackState);
    }

    private void OnJumpPressed()
    {
        if (_player.AirJumpsRemaining <= 0) { return; }

        
        _stateMachine.ChangeState(_stateMachine.AirJumpState);
    }

    private void OnJumpReleased()
    {
        // cut the jump short on release
        _movement.MoveY(_movement.Velocity.y * _data.ShortJumpHeightScale);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.IsReady)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
