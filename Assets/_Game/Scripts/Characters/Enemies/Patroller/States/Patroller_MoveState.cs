using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_MoveState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private MovementKM _movement;
    private RayDetector _playerDetector;
    private OverlapDetector _wallDetector;
    private OverlapDetector _groundDetector;
    private OverlapDetector _groundInFrontDetector;

    public Patroller_MoveState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _playerDetector = patroller.AggroDetector;
        _wallDetector = patroller.WallDetector;
        _groundDetector = patroller.GroundDetector;
        _groundInFrontDetector = patroller.SpaceDetector;
    }

    public override void Enter()
    {
        _movement.MoveX(_data.MovementSpeed * _movement.FacingDirection, true);

        _playerDetector.StartDetecting();
        _wallDetector.StartDetecting();
        _groundDetector.StartDetecting();
        _groundInFrontDetector.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerDetector.StopDetecting();
        _wallDetector.StopDetecting();
        _groundDetector.StopDetecting();
        _groundInFrontDetector.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if we've reached the path end
        bool isLedge = !_groundInFrontDetector && _groundDetector.IsDetected;
        if (isLedge || _wallDetector.IsDetected)
        {
            // idle if specified
            if (_data.IdleOnPathEnd)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
            // otherwise turn and continue
            else
            {
                _movement.Flip();
                _movement.MoveX(_data.MovementSpeed 
                    * _movement.FacingDirection, true);
            }
        }
        // if we've detected the player
        if (_playerDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
