using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_SearchState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private MovementKM _movement;
    private RayDetector _playerLOS;

    private float _lastTurnTime;
    private int _turnsCompleted;

    public Patroller_SearchState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _playerLOS = patroller.PlayerDetector.PlayerLOS;
    }

    public override void Enter()
    {
        base.Enter();

        _lastTurnTime = 0;
        _turnsCompleted = 0;

        _movement.MoveX(0, false);

        // do the first turn immediately
        if(_data.TurnImmediatelyOnSearch && _data.NumberOfSearchTurns >= 1)
        {
            Turn();
        }

        _playerLOS.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerLOS.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_playerLOS.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();

        _lastTurnTime += Time.deltaTime;
        // if our time during this turn has been exceeded, turn again or go back to patrolling
        if (_lastTurnTime >= _data.SearchTurnDuration)
        {
            if(_turnsCompleted < _data.NumberOfSearchTurns)
            {
                Turn();
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.MoveState);
                return;
            }
        }
    }

    private void Turn()
    {
        _lastTurnTime = 0;
        _turnsCompleted++;
        _movement.Flip();
    }
}
