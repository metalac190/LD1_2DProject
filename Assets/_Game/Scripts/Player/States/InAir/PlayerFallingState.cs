using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;
    LedgeDetector _ledgeDetector;

    bool _lateJumpAllowed = false;
    bool _lateWallJumpAllowed = false;

    public PlayerFallingState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
        _wallDetector = player.WallDetector;
        _ledgeDetector = player.LedgeDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Falling");
        _input.JumpPressed += OnJumpPressed;
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

        _lateJumpAllowed = false;
        _lateWallJumpAllowed = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        _wallDetector.DetectWall();
        _ledgeDetector.DetectUpperLedge();
        

        // check for ledge grab - prioritze over wall grab
        if (_ledgeDetector.IsDetectingUpperLedge)
        {
            _stateMachine.ChangeState(_stateMachine.LedgeHangState);
            return;
        }
        // otherwise, check for wall grab
        else if (_wallDetector.IsWallDetected
            && _input.XRaw == _player.FacingDirection)
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
        else if (_groundDetector.IsGrounded && _player.RB.velocity.y < 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);

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

    private void OnJumpPressed()
    {
        if(_player.AirJumpsRemaining <= 0) { return; }
        // if we have remaining jumps, determine if it is a air jump or a wall jump
        if (_lateWallJumpAllowed)
        {
            // if we're facing away from the wall, flip before wall jumping
            if (!_wallDetector.IsWallDetected)
            {
                _player.Flip();
            }
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
            return;
        }
        // otherwise do a normal air jump
        else
        {
            _stateMachine.ChangeState(_stateMachine.AirJumpState);
            return;
        }
    }
}
