using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSuperState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    GroundDetector _groundDetector;
    WallDetector _wallDetector;

    public PlayerWallSuperState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _groundDetector = player.GroundDetector;
        _wallDetector = player.WallDetector;
    }

    public override void Enter()
    {
        base.Enter();
        _input.JumpPressed += OnJumpPressed;

        _player.ResetJumps();
    }

    public override void Exit()
    {
        base.Exit();
        _input.JumpPressed -= OnJumpPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_wallDetector.IsAgainstWall)
        {
            OnLostWall();
        }
    }

    public override void Update()
    {
        base.Update();
        // if we're not holding against the wall, lose it
        if (_input.XRaw != _player.FacingDirection)
        {
            OnLostWall();
        }

    }

    private void OnLostWall()
    {
        Debug.Log("Lost the wall");
        if (_groundDetector.IsGrounded)
        {
            _stateMachine.ChangeState(_stateMachine.LandState);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }
    }

    private void OnJumpPressed()
    {
        _stateMachine.ChangeState(_stateMachine.WallJumpState);
    }
}
