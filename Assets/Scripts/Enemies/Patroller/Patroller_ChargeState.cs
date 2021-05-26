using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_ChargeState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PatrollerData _data;

    EnvironmentDetector _environmentDetector;
    PlayerDetector _playerDetector;

    public Patroller_ChargeState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _environmentDetector = patroller.EnvironmentDetector;
        _playerDetector = patroller.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _patroller.Move(_data.ChargeSpeed);

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
            _stateMachine.ChangeState(_stateMachine.SearchState);
        }
        else if (_playerDetector.IsPlayerInCloseRange)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }

        //TODO: if player is in melee range, do attack

        if (StateDuration >= _data.ChargeDuration)
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
