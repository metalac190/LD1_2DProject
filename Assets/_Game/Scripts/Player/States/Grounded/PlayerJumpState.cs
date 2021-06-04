using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    PlayerData _data;
    GroundDetector _groundDetector;
    DashSystem _dashSystem;
    PlayerSFXData _sfx;

    public PlayerJumpState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.GroundDetector;
        _dashSystem = player.DashSystem;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Jump");

        _input.JumpPressed += OnJumpPressed;
        _input.JumpReleased += OnJumpReleased;
        _input.DashPressed += OnDashPressed;

        _player.SetVelocityY(_data.JumpVelocity);

        _sfx.JumpSFX.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.JumpReleased -= OnJumpReleased;
        _input.DashPressed -= OnDashPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();
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

    private void OnJumpPressed()
    {
        if (_player.AirJumpsRemaining <= 0) { return; }

        _stateMachine.ChangeState(_stateMachine.AirJumpState);
    }

    private void OnJumpReleased()
    {
        // cut the jump short on release
        _player.SetVelocityY(_player.RB.velocity.y * _data.ShortJumpHeightScale);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
