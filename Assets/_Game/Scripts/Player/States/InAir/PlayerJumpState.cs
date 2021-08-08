using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;
    private PlayerAnimator _animator;

    private MovementKM _movement;
    private GameplayInput _input;
    private PlayerData _data;
    private OverlapDetector _groundDetector;
    private DashSystem _dashSystem;
    private PlayerSFXData _sfx;

    public PlayerJumpState(PlayerFSM stateMachine, Player player)
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
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log("STATE: Jump");
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

        _groundDetector.Detect();
        // if we're not grounded, but began falling, go to fall state
        if(_movement.Velocity.y <= 0)
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
