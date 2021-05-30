using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;
    GroundDetector _groundDetector;

    public PlayerIdleState(PlayerFSM stateMachine, Player player) 
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
        Debug.Log("STATE: Idle");
        _input.SpacebarPressed += OnSpacebarPressed;
        _groundDetector.LeftGround += OnLeftGround;

        _player.SetVelocityX(0);
    }

    public override void Exit()
    {
        base.Exit();

        _input.SpacebarPressed -= OnSpacebarPressed;
        _groundDetector.LeftGround -= OnLeftGround;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(_input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }

    private void OnSpacebarPressed()
    {
        //if (_player.JumpsRemaining <= 0) { return; }

        if (_data.AllowJump)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    private void OnLeftGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
