using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : State
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    MovementKM _movement;
    PlayerMoveData _data;
    GameplayInput _input;
    OverlapDetector _groundDetector;
    OverlapDetector _wallDetector;
    OverlapDetector _aboveWallDetector;

    DashSystem _dashSystem;

    bool _lateJumpAllowed = false;
    bool _lateWallJumpAllowed = false;

    public PlayerFallingState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.EnvironmentDetector.GroundDetector;
        _wallDetector = player.EnvironmentDetector.WallDetector;
        _aboveWallDetector = player.EnvironmentDetector.AboveWallDetector;

        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Falling");

        _animator.PlayAnimation(PlayerAnimator.FallName);

        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        // reset our physics checks just in case we haven't updated since last frame
        _wallDetector.Detect();
        _aboveWallDetector.Detect();
        _groundDetector.Detect();
        // alow the player a free jump if they've recently left ground (Coyote time)
        if(_groundDetector.LostDetectionDuration <= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = true;
        }
        // or if they've recently left the wall
        if(_wallDetector.LostDetectionDuration <= _data.WallJumpAfterFallDuration)
        {
            _lateWallJumpAllowed = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;

        _lateJumpAllowed = false;
        _lateWallJumpAllowed = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.Detect();
        _wallDetector.Detect();
        _aboveWallDetector.Detect();


        // check for wall grab
        if (_wallDetector.IsDetected
            && _input.XInputRaw == _movement.FacingDirection)
        {
            Debug.Log("Wall detected!");
            // determine if we can enter any of our wall states
            if (_data.AllowWallClimb)
            {
                _stateMachine.ChangeState(_stateMachine.WallClimbState);
                return;
            }
            else if (_data.AllowWallGrab)
            {
                _stateMachine.ChangeState(_stateMachine.WallGrabState);
                return;
            }
            else if (_data.AllowWallSlide)
            {
                _stateMachine.ChangeState(_stateMachine.WallSlideState);
                return;
            }
        }

        // check for grounded
        else if (_groundDetector.IsDetected && _movement.Velocity.y < 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
            return;
        }

        _movement.MoveX(_input.XInputRaw * _data.MoveSpeed, true);
    }

    public override void Update()
    {
        base.Update();

        // if lateJump is allowed, and we've passed the window, close it off
        // if we're past the allow late jump window, then close it off and remove our buffer jump
        CheckLateJump();
        // if late wall jump is allowed, and we've passed the window, close it off
        CheckLateWallJump();
    }

    private void CheckLateWallJump()
    {
        if (_lateWallJumpAllowed
                    && _wallDetector.LostDetectionDuration 
                    >= _data.WallJumpAfterFallDuration)
        {
            _lateWallJumpAllowed = false;
        }
    }

    private void CheckLateJump()
    {
        if (_lateJumpAllowed
                    && _groundDetector.LostDetectionDuration 
                    >= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = false;
        }
    }

    private void OnAttackPressed()
    {
        if(_input.YInputRaw == -1)
        {
            _stateMachine.ChangeState(_stateMachine.BounceAttackState);
            return;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.AirAttackState);
            return;
        }
        
    }

    private void OnJumpPressed()
    {
        if (_wallDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
            return;
        }
        // test for wall jump
        else if (_wallDetector.IsDetected == false && _lateWallJumpAllowed)
        {
            // if we're facing away from the wall, flip before wall jumping
            _movement.Flip();
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
            return;
        }
        // otherwise do a normal air jump, if we have some remaining
        else if (_player.AirJumpsRemaining > 0)
        {
            _stateMachine.ChangeState(_stateMachine.AirJumpState);
            return;
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
