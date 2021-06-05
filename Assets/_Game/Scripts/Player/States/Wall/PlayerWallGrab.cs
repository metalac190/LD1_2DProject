using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrab : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    PlayerData _data;
    GameplayInput _input;
    PlayerSFXData _sfx;

    Vector2 _startPosition;

    public PlayerWallGrab(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _input = player.Input;
        _data = player.Data;
        _sfx = player.SFX;
    }

    //
    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Wall Grab");

        _startPosition = _movement.Position;

        _sfx.WallGrabSFX.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _movement.HoldPosition(_startPosition);
    }

    public override void Update()
    {
        base.Update();
        // if we're allowed to climb, test climb input
        if (_data.AllowWallClimb && _input.YInputRaw > 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallClimbState);
            return;
        }
        // if we're inputting down, slide
        if (_data.AllowWallSlide && _input.YInputRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallSlideState);
            return;
        }
    }
}
