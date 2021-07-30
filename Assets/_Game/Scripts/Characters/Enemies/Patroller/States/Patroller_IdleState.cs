using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_IdleState : State
{
    private float _idleTime = 0;

    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PatrollerData _data;

    PlayerDetector _playerDetector;
    EnvironmentDetector _environmentDetector;

    public Patroller_IdleState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _playerDetector = patroller.PlayerDetector;
        _environmentDetector = patroller.EnvironmentDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _patroller.Move(0);
        SetRandomIdleTime();

        _playerDetector.StartCheckingForPlayer();
    }

    public override void Exit()
    {
        base.Exit();

        _playerDetector.StopCheckingForPlayer();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();


        if (StateDuration >= _idleTime)
        {
            // if we're at a wall or ledge, turn around before moving
            if (_environmentDetector.CheckLedge() || _environmentDetector.CheckWall())
            {
                _patroller.Flip();
            }
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }

        if (_playerDetector.IsPlayerDetected)
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
