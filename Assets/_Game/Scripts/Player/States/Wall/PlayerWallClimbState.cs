using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    PlayerData _data;
    GameplayInput _input;
    LedgeDetector _ledgeDetector;

    public PlayerWallClimbState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _input = player.Input;
        _ledgeDetector = player.Actor.CollisionDetector.LedgeDetector;
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

        _movement.SetVelocityY(_data.WallClimbVelocity);

        if (_ledgeDetector.IsDetectingUpperLedge && _data.AllowLedgeHang)
        {
            _stateMachine.ChangeState(_stateMachine.LedgeHangState);
        }
    }

    public override void Update()
    {
        base.Update();
        // test for wall grab
        
        if(_data.AllowWallGrab && _input.YInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallGrabState);
        }
        // test for wall slide
        else if(_data.AllowWallSlide && _input.YInputRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallSlideState);
        }
    }
}
