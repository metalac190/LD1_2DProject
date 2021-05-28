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

    bool _lateJumpAllowed = false;

    public PlayerFallingState(PlayerFSM stateMachine, Player player)
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

        Debug.Log("STATE: Falling");
        _groundDetector.FoundGround += OnFoundGround;
        _input.SpacebarPressed += OnSpacebarPressed;
        // alow the player a free jump if they've recently left ground (Coyote time)
        if(_groundDetector.TimeInAir <= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        _groundDetector.FoundGround -= OnFoundGround;
        _input.SpacebarPressed -= OnSpacebarPressed;

        _lateJumpAllowed = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);

        // if lateJump is allow, and we've passed the window, close it off
        // if we're past the allow late jump window, then close it off and remove our buffer jump
        if(_lateJumpAllowed 
            && _groundDetector.TimeInAir >= _data.JumpAfterFallDuration)
        {
            _lateJumpAllowed = false;
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
        // if we have remaining jumps, do a mid-air jump!
        _stateMachine.ChangeState(_stateMachine.JumpingState);
    }
}
