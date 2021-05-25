using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_IdleState : State
{
    private float _idleTime = 0;

    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PlayerDetector _playerDetector;
    EnvironmentDetector _environmentDetector;

    public Patroller_IdleState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _playerDetector = patroller.PlayerDetector;
        _environmentDetector = patroller.EnvironmentDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _patroller.SetVelocity(0);
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
        _idleTime = UnityEngine.Random.Range(_patroller.MinIdleTime, _patroller.MaxIdleTime);
    }
}
