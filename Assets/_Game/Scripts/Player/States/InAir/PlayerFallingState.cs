using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;
    LedgeDetector _ledgeDetector;

    DashSystem _dashSystem;

    bool _lateJumpAllowed = false;
    bool _lateWallJumpAllowed = false;

    public PlayerFallingState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.Actor.CollisionDetector.GroundDetector;
        _wallDetector = player.Actor.CollisionDetector.WallDetector;
        _ledgeDetector = player.Actor.CollisionDetector.LedgeDetector;

        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Falling");

        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        // reset our physics checks just in case we haven't updated since last frame
        _wallDetector.DetectWall();
        _ledgeDetector.DetectUpperLedge();
        _groundDetector.DetectGround();
        // alow the player a free jump if they've recently left ground (Coyote time)
        if(_groundDetector.TimeInAir <= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = true;
        }
        if(_wallDetector.TimeOffWall <= _data.WallJumpAfterFallDuration)
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

        _groundDetector.DetectGround();
        _wallDetector.DetectWall();
        _ledgeDetector.DetectUpperLedge();

        _movement.MoveX(_input.XInputRaw * _data.MoveSpeed);

        // check for ledge grab - prioritze over wall grab
        if (_ledgeDetector.IsDetectingUpperLedge && _data.AllowLedgeHang)
        {
            _stateMachine.ChangeState(_stateMachine.LedgeHangState);
            return;
        }
        // otherwise, check for wall grab
        else if (_wallDetector.IsWallDetected
            && _input.XInputRaw == _movement.FacingDirection)
        {
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
        else if (_groundDetector.IsGrounded && _movement.Velocity.y < 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
            return;
        }
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
                    && _wallDetector.TimeOffWall >= _data.WallJumpAfterFallDuration)
        {
            _lateWallJumpAllowed = false;
        }
    }

    private void CheckLateJump()
    {
        if (_lateJumpAllowed
                    && _groundDetector.TimeInAir >= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = false;
        }
    }

    private void OnAttackPressed()
    {
        _stateMachine.ChangeState(_stateMachine.AirAttackState);
    }

    private void OnJumpPressed()
    {
        if (_wallDetector.IsWallDetected)
        {
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
            return;
        }
        // test for wall jump
        else if (!_wallDetector.IsWallDetected && _lateWallJumpAllowed)
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
