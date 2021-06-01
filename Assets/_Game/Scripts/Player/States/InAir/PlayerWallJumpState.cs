using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;

    // this prevents player from immediately moving back into wall while wall jumping
    bool _isMoveInputAllowed = false;

    public PlayerWallJumpState(PlayerFSM stateMachine, Player player)
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
        Debug.Log("STATE: Wall Jump");

        _input.JumpPressed -= OnJumpPressed;

        _isMoveInputAllowed = false;

        //_player.DecreaseAirJumpsRemaining();
        Debug.Log("Remaining Jumps: " + _player.AirJumpsRemaining);
        // reverse direction
        _player.SetVelocity(_data.WallJumpVelocity, _data.WallJumpAngle, -_player.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        _wallDetector.DetectWall();

        // if we're not grounded, but began falling, go to fall state
        if (!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }

        if (_isMoveInputAllowed)
        {
            _player.SetVelocityX(_input.XRaw * _data.MoveSpeed * _data.WallJumpMovementDampener);
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

    private void OnJumpPressed()
    {
        Debug.Log("Jump pressed");
        // if we're detecting another wall immediately do another wall jump
        if(_wallDetector.IsWallDetected)
        {
            _stateMachine.ChangeState(_stateMachine.WallJumpState);
        }
        // otherwise, if we have remaining air jumps, use that
        else if(_player.AirJumpsRemaining >= 0)
        {
            _stateMachine.ChangeState(_stateMachine.AirJumpState);
        }
    }
}
