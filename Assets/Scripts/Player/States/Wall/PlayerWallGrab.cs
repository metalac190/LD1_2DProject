using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrab : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;

    Vector2 _startPosition;

    public PlayerWallGrab(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
    }

    //
    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Wall Grab");

        _startPosition = _player.RB.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _player.HoldPosition(_startPosition);
    }

    public override void Update()
    {
        base.Update();
        // if we're allowed to climb, test climb input
        if (_data.AllowWallClimb && _input.YRaw > 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallClimbState);
        }
        // if we're inputting down, slide
        if (_data.AllowWallSlide && _input.YRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallSlideState);
        }
    }
}
