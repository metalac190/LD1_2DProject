using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    PlayerData _data;
    GroundDetector _groundDetector;

    public PlayerAirJumpState(PlayerFSM stateMachine, Player player)
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

        _input.JumpReleased += OnJumpReleased;

        _player.DecreaseAirJumpsRemaining();
        _player.SetVelocityY(_data.AirJumpVelocity);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpReleased -= OnJumpReleased;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
        // if we're not grounded, but began falling, go to fall state
        if (!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }

        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnJumpReleased()
    {
        // cut the jump short on release
        _player.SetVelocityY(_player.RB.velocity.y * _data.ShortAirJumpHeightScale);
    }
}
