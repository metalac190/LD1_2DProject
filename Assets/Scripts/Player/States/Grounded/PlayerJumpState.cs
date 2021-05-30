using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    InputManager _input;
    PlayerData _data;
    GroundDetector _groundDetector;

    public PlayerJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Jump");

        _input.SpacebarReleased += OnSpacebarReleased;

        //_player.DecreaseAirJumpsRemaining();
        Debug.Log("Remaining Jumps: " + _player.AirJumpsRemaining);
        _player.SetVelocityY(_data.JumpVelocity);

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
        if(!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }

        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);
    }

    public override void Update()
    {
        base.Update();
        
        
    }

    private void OnSpacebarReleased()
    {
        // cut the jump short on release
        _player.SetVelocityY(_player.RB.velocity.y * _data.ShortJumpHeightScale);
    }
}
