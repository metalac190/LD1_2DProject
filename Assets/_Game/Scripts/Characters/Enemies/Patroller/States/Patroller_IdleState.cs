using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_IdleState : State
{
    private float _idleTime = 0;

    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private MovementKM _movement;
    private RayDetector _playerInRange;
    private OverlapDetector _wallDetector;
    private OverlapDetector _groundDetector;
    private OverlapDetector _groundInFrontDetector;

    public Patroller_IdleState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _playerInRange = patroller.PlayerDetector.PlayerLOS;
        _wallDetector = patroller.EnvironmentDetector.WallDetector;
        _groundDetector = patroller.EnvironmentDetector.GroundDetector;
        _groundInFrontDetector = patroller.EnvironmentDetector.GroundInFrontDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _movement.MoveX(0, false);
        SetRandomIdleTime();

        _playerInRange.StartDetecting();
        _wallDetector.StartDetecting();
        _groundDetector.StartDetecting();
        _groundInFrontDetector.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerInRange.StopDetecting();
        _wallDetector.StopDetecting();
        _groundDetector.StopDetecting();
        _groundInFrontDetector.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (StateDuration >= _idleTime)
        {
            // if we detect space in front but are grounded, it's a ledge
            bool isLedge = !_groundInFrontDetector.IsDetected 
                && _groundDetector.IsDetected;
            if (isLedge || _wallDetector.IsDetected)
            {
                _movement.Flip();
            }
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }

        if (_playerInRange.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
        }
    }

    public override void Update()
    {
        base.Update();

    }

    private void SetRandomIdleTime()
    {
        _idleTime = UnityEngine.Random.Range(_data.MinIdleTime, _data.MaxIdleTime);
    }
}
