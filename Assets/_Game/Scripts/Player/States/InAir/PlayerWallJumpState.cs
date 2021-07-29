using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    MovementKM _movement;
    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;

    // this prevents player from immediately moving back into wall while wall jumping
    bool _isMoveInputAllowed = false;

    public PlayerWallJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.CollisionDetector.GroundDetector;
        _wallDetector = player.CollisionDetector.WallDetector;
        _dashSystem = player.DashSystem;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Wall Jump");
        _animator.PlayAnimation(PlayerAnimator.JumpName);

        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;

        _isMoveInputAllowed = false;

        //_player.DecreaseAirJumpsRemaining();
        Debug.Log("Remaining Jumps: " + _player.AirJumpsRemaining);
        // reverse direction
        _movement.SetVelocityZero();
        _movement.Flip();
        _movement.Move(_data.WallJumpVelocity, _data.WallJumpAngle, _movement.FacingDirection, false);

        _sfx.JumpSFX?.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        _wallDetector.DetectWall();

        // if we're not grounded, but began falling, go to fall state
        if (!_groundDetector.IsGrounded && _movement.Velocity.y < 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }
        else if (_wallDetector.IsWallDetected)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }

        // if movement is now allowed, adjust player 
        if (_isMoveInputAllowed)
        {
            _movement.MoveX(_input.XInputRaw * _data.MoveSpeed * _data.WallJumpMovementDampener, true);
        }
        else
        {
            _movement.MoveX(_data.MoveSpeed * _movement.FacingDirection, false);
        }
    }

    public override void Update()
    {
        base.Update();

        // if we've waited the lock duration, unlock input
        if(!_isMoveInputAllowed && StateDuration > _data.MoveInputLockDuration)
        {
            _isMoveInputAllowed = true;
        }
    }

    private void OnAttackPressed()
    {
        _stateMachine.ChangeState(_stateMachine.AirAttackState);
    }

    private void OnJumpPressed()
    {
        Debug.Log("Jump pressed");
        // if we're detecting another wall immediately do another wall jump
        if(_wallDetector.IsWallDetected)
        {
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
        }
        // otherwise, if we have remaining air jumps, use that
        else if(_player.AirJumpsRemaining > 0)
        {
            _stateMachine.ChangeState(_stateMachine.AirJumpState);
        }
    }

    private void OnDashPressed()
    {
        if (_dashSystem.IsReady)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
