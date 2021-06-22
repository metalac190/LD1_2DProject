using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    KinematicObject _movement;
    PlayerData _data;
    GameplayInput _input;

    private float _accelerationAmount = 0;

    public PlayerWallSlideState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Wall Slide");
        _accelerationAmount = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float newSlideVelocity = -(_data.WallSlideVelocity + _accelerationAmount);
        _movement.MoveY(newSlideVelocity);
        _accelerationAmount += _data.WallSlideAcceleration;
    }

    public override void Update()
    {
        base.Update();
        // if player is allowed to climb the wall, test the input and climb
        if (_data.AllowWallClimb && _input.YInputRaw > 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallClimbState);
        }
        // if the player is allowed to grab the wall, test input and grab
        else if(_data.AllowWallGrab && _input.YInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallGrabState);
        }
        
    }
}
