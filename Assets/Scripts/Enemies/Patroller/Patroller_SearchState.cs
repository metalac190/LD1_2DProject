using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_SearchState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PlayerDetector _playerDetector;

    private float _lastTurnTime;
    private int _turnsCompleted;

    public Patroller_SearchState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _playerDetector = patroller.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _lastTurnTime = 0;
        _turnsCompleted = 0;

        _patroller.SetVelocity(0);

        // do the first turn immediately
        if(_patroller.NumberOfSearchTurns >= 1)
        {
            Turn();
        }

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

        if (_playerDetector.IsPlayerDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
        }
    }

    public override void Update()
    {
        base.Update();

        _lastTurnTime += Time.deltaTime;
        // if our time during this turn has been exceeded, turn again or go back to patrolling
        if (_lastTurnTime >= _patroller.SearchTurnDuration)
        {
            if(_turnsCompleted < _patroller.NumberOfSearchTurns)
            {
                Turn();
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.MoveState);
            }
        }
    }

    private void Turn()
    {
        _lastTurnTime = 0;
        _turnsCompleted++;
        _patroller.Flip();
    }
}
