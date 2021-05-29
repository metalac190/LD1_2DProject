using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    InputManager _input;

    public PlayerWallClimbState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Wall Climb");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _player.SetVelocityY(_data.WallClimbVelocity);
    }

    public override void Update()
    {
        base.Update();
        // test for wall grab
        if(_data.AllowWallGrab && _input.YRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallGrabState);
        }
        // test for wall slide
        else if(_data.AllowWallSlide && _input.YRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallSlideState);
        }
    }
}
