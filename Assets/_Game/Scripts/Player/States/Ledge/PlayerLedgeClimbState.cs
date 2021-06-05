using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    PlayerData _data;
    LedgeDetector _ledgeDetector;
    CeilingDetector _ceilingDetector;

    private Vector2 _cornerPos;
    private Vector2 _hangPosition;
    private Vector2 _stopClimbPos;

    private bool _isTouchingCeiling = false;

    private float _standupTimer = 0;
    private bool _finishClimb = false;

    public PlayerLedgeClimbState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _ledgeDetector = player.Actor.CollisionDetector.LedgeDetector;
        _ceilingDetector = player.Actor.CollisionDetector.CeilingDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Ledge Climb");

        _isTouchingCeiling = false;
        _finishClimb = false;

        _cornerPos = _ledgeDetector.CalculateUpperLedgeCornerPosition(_movement.FacingDirection);

        CalculateClimbPositions();
        CheckForCeiling();
        // set initial player position
        _movement.MovePositionInstant(_hangPosition);
        _movement.HoldPosition(_hangPosition);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _movement.HoldPosition(_hangPosition);
        if (_finishClimb)
        {
            FinishClimb();
        }
    }

    public override void Update()
    {
        base.Update();

        // once climb duration is completed, move on top of ledge
        if(!_finishClimb && StateDuration >= _data.LedgeClimbDuration)
        {
            _finishClimb = true;
        }
    }

    private void FinishClimb()
    {
        _movement.MovePositionInstant(_stopClimbPos);

        _movement.HoldPosition(_stopClimbPos);

        if (_isTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }

    private void CalculateClimbPositions()
    {
        // calculate start climb from player offsets
        _hangPosition = new Vector2(_cornerPos.x - (_movement.FacingDirection * _data.StartClimbOffset.x),
            _cornerPos.y - _data.StartClimbOffset.y);
        // calculate stop climb from player offsets
        _stopClimbPos = new Vector2(_cornerPos.x + (_movement.FacingDirection * _data.StopClimbOffset.x),
            _cornerPos.y + _data.StopClimbOffset.y);
    }

    private void CheckForCeiling()
    {
        // check above the corner position
        _isTouchingCeiling = Physics2D.Raycast(_cornerPos + (Vector2.up * 0.1f)
            + (Vector2.right * _movement.FacingDirection * 0.15f), 
            Vector2.up, _data.StandColliderHeight, _ceilingDetector.WhatIsCeiling);
    }
}
