using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    MovementKM _movement;
    PlayerMoveData _data;
    GameplayInput _input;
    OverlapDetector _aboveWallDetector;
    OverlapDetector _wallDetector;

    public PlayerWallClimbState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _aboveWallDetector = player.EnvironmentDetector.AboveWallDetector;
        _wallDetector = player.EnvironmentDetector.WallDetector;
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

        _aboveWallDetector.Detect();

        _movement.MoveY(_data.WallClimbVelocity);

        if (_aboveWallDetector.IsDetected == false && _data.AllowLedgeHop)
        {
            // space above wall, if wall in front of us it's an upper ledge!
            if(_wallDetector.Detect() != null)
            {
                _stateMachine.ChangeState(_stateMachine.LedgeClimbState);
                return;
            }
        }
    }

    public override void Update()
    {
        base.Update();
        // test for wall grab
        
        if(_data.AllowWallGrab && _input.YInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallGrabState);
            return;
        }
        // test for wall slide
        else if(_data.AllowWallSlide && _input.YInputRaw < 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallSlideState);
            return;
        }
    }
}
