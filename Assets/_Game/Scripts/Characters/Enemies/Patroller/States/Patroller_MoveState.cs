using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_MoveState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private MovementKM _movement;
    private RayDetector _playerLOS;
    private OverlapDetector _wallDetector;
    private OverlapDetector _groundDetector;
    private OverlapDetector _groundInFrontDetector;

    public Patroller_MoveState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _playerLOS = patroller.PlayerDetector.PlayerLOS;
        _wallDetector = patroller.EnvironmentDetector.WallDetector;
        _groundDetector = patroller.EnvironmentDetector.GroundDetector;
        _groundInFrontDetector = patroller.EnvironmentDetector.GroundInFrontDetector;
    }

    public override void Enter()
    {
        //Debug.Log("PATROLLER: Move State");
        _movement.MoveX(_data.MovementSpeed * _movement.FacingDirection, true);

        _playerLOS.StartDetecting();
        _wallDetector.StartDetecting();
        _groundDetector.StartDetecting();
        _groundInFrontDetector.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerLOS.StopDetecting();
        _wallDetector.StopDetecting();
        _groundDetector.StopDetecting();
        _groundInFrontDetector.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // look for wall
        if (_wallDetector.IsDetected)
        {
            HandleEndOfPath();
        }
        // look for ledge
        else if (!_groundInFrontDetector.IsDetected)
        {
            // if there's nothing in front but we're grounded, it's a ledge
            if (_groundDetector.Detect() != null)
            {
                HandleEndOfPath();
            }
        }
        // look for player
        else if (_playerLOS.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
            return;
        }
        // otherwise, keep moving
        else
        {
            _movement.MoveX(_data.MovementSpeed
                * _movement.FacingDirection, true);
        }
    }

    private void HandleEndOfPath()
    {
        // idle if specified
        if (_data.IdleOnPathEnd)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
        // otherwise turn and continue
        else
        {
            _movement.Flip();
            _movement.MoveX(_data.MovementSpeed
                * _movement.FacingDirection, true);
            // immediately detect new direction
            _groundInFrontDetector.Detect();
            _wallDetector.Detect();
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
