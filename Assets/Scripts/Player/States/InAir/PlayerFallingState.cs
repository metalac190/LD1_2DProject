using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;

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
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Falling");
        _groundDetector.FoundGround += OnFoundGround;
        _input.SpacebarPressed += OnSpacebarPressed;
        // alow the player a free jump if they've recently left ground (Coyote time)
        if(_groundDetector.TimeInAir <= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = true;
        }
        if(_wallDetector.TimeOffWall <= _data.WallJumpAfterFallDuration)
        {
            Debug.Log("Late wall jump locked");
            _lateWallJumpAllowed = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        _groundDetector.FoundGround -= OnFoundGround;
        _input.SpacebarPressed -= OnSpacebarPressed;

        _lateJumpAllowed = false;
        _lateWallJumpAllowed = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckForWallGrab();
    }

    private void CheckForWallGrab()
    {
        if (_wallDetector.IsAgainstWall
            && _input.XRaw == _player.FacingDirection)
        {
            // determine if we can enter any of our wall states
            if (_data.AllowWallClimb)
            {
                _stateMachine.ChangeState(_stateMachine.WallClimbState);
            }
            else if (_data.AllowWallGrab)
            {
                _stateMachine.ChangeState(_stateMachine.WallGrabState);
            }
            else if (_data.AllowWallSlide)
            {
                _stateMachine.ChangeState(_stateMachine.WallSlideState);
            }
        }
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);

        // if lateJump is allowed, and we've passed the window, close it off
        // if we're past the allow late jump window, then close it off and remove our buffer jump
        if(_lateJumpAllowed 
            && _groundDetector.TimeInAir >= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = false;
            _player.DecreaseJumpsRemaining();
        }
        // if late wall jump is allowed, and we've passed the window, close it off
        if(_lateWallJumpAllowed 
            && _wallDetector.TimeOffWall >= _data.WallJumpAfterFallDuration)
        {
            _lateWallJumpAllowed = false;
            _player.DecreaseJumpsRemaining();
        }
    }

    private void OnFoundGround()
    {
        // if we've just hit ground and our velocity is downwards, we've landed
        if(_player.RB.velocity.y < 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
        }
    }

    private void OnSpacebarPressed()
    {
        if(_player.JumpsRemaining <= 0) { return; }
        // if we have remaining jumps, determine if it is a air jump or a wall jump
        if (_lateWallJumpAllowed)
        {
            // if we're facing away from the wall, flip before wall jumping
            if (!_wallDetector.IsAgainstWall)
            {
                _player.Flip();
            }
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
        }
        // otherwise do a normal air jump
        else
        {
            _stateMachine.ChangeState(_stateMachine.JumpingState);
        }
    }
}
