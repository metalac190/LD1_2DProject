using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrab : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;

    Vector2 _holdPosition;

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

        _holdPosition = _player.RB.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        HoldPosition();
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

    private void HoldPosition()
    {
        _player.SetVelocityY(0);
        _player.RB.MovePosition(_holdPosition);
    }
}
