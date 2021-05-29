using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;
    GroundDetector _groundDetector;

    // this prevents player from immediately moving back into wall while wall jumping
    bool _isInputAllowed = false;

    public PlayerWallJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Wall Jump");

        _input.SpacebarReleased += OnSpacebarReleased;

        _isInputAllowed = false;

        _player.DecreaseJumpsRemaining();
        Debug.Log("Remaining Jumps: " + _player.JumpsRemaining);
        // reverse direction
        _player.SetVelocity(_data.WallJumpVelocity, _data.WallJumpAngle, -_player.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();

        _input.SpacebarReleased -= OnSpacebarReleased;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if we're not grounded, but began falling, go to fall state
        if (!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }

        if (_isInputAllowed)
        {
            _player.SetVelocityX(_input.XRaw * _data.MoveSpeed * _data.WallJumpMovementDampener);
        }
        
    }

    public override void Update()
    {
        base.Update();

        // if we've waited the lock duration, unlock input
        if(!_isInputAllowed && StateDuration > _data.MoveInputLockDuration)
        {
            _isInputAllowed = true;
        }
    }


    private void OnSpacebarReleased()
    {
        //_player.SetVelocityY(_player.RB.velocity.y * _data.ShortJumpHeightMultiplier);
    }
}
