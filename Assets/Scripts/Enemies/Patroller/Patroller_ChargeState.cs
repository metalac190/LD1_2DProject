using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_ChargeState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    EnvironmentDetector _environmentDetector;
    PlayerDetector _playerDetector;

    public Patroller_ChargeState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _environmentDetector = patroller.EnvironmentDetector;
        _playerDetector = patroller.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _patroller.SetVelocity(_patroller.ChargeSpeed);

        _environmentDetector.StartCheckingEnvironment();
        _playerDetector.StartCheckingForPlayer();
    }

    public override void Exit()
    {
        base.Exit();

        _environmentDetector.StopCheckingEnvironment();
        _playerDetector.StopCheckingForPlayer();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_environmentDetector.IsWallDetected || _environmentDetector.IsLedgeDetected)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }

        if (StateDuration >= _patroller.ChargeTime)
        {
            // if player still detected, do it again
            if (_playerDetector.IsPlayerDetected)
            {
                _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
            }
            // otherwise transition back to patrol
            else
            {
                _stateMachine.ChangeState(_stateMachine.SearchState);
            }
        }
    }

    public override void Update()
    {
        base.Update();

    }
}
