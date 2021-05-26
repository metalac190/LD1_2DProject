using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_MoveState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private PlayerDetector _playerDetector;
    private EnvironmentDetector _environmentDetector;

    public Patroller_MoveState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _playerDetector = patroller.PlayerDetector;
        _environmentDetector = patroller.EnvironmentDetector;
    }

    public override void Enter()
    {
        _patroller.Move(_data.MovementSpeed);

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

        // if we've reached the path end
        if (_environmentDetector.IsWallDetected || _environmentDetector.IsLedgeDetected)
        {
            // idle if specified
            if (_data.IdleOnPathEnd)
            {
                Debug.Log("Patroller: Moving and detected ledge or wall!");
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
            // otherwise turn and continue
            else
            {
                _patroller.Flip();
                _patroller.Move(_data.MovementSpeed);
            }
        }
        // if we've detected the player
        if (_playerDetector.IsPlayerDetected)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
