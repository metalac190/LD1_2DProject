using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerWallSuperState
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    MovementKM _movement;
    PlayerMoveData _data;
    GameplayInput _input;
    ParticleSystem _jumpDust;

    private float _accelerationAmount = 0;

    public PlayerWallSlideState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _jumpDust = player.Visuals.JumpDust;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Wall Slide");
        _animator.PlayAnimation(PlayerAnimator.WallSlideName);
        _accelerationAmount = 0;

        _jumpDust?.Play();
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
            return;
        }
        // if the player is allowed to grab the wall, test input and grab
        else if(_data.AllowWallGrab && _input.YInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.WallGrabState);
            return;
        }
        
    }
}
