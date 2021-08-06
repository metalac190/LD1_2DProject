using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_ChargeState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PatrollerData _data;

    private MovementKM _movement;
    private ColliderDetector _groundDetector;
    private ColliderDetector _wallDetector;
    private ColliderDetector _lowerLedgeDetector;
    private RayDetector _playerDetector;
    GameObject _detectedGraphic;

    public Patroller_ChargeState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _groundDetector = patroller.GroundDetector;
        _wallDetector = patroller.WallDetector;
        _lowerLedgeDetector = patroller.SpaceDetector;
        _playerDetector = patroller.AggroDetector;
        _detectedGraphic = patroller.DetectedGraphic;
    }

    public override void Enter()
    {
        base.Enter();



        _detectedGraphic.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        _detectedGraphic.SetActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.Detect();
        _playerDetector.Detect();

        _movement.MoveX(_data.ChargeSpeed * _movement.FacingDirection, true);

        if (_wallDetector.IsDetected || _lowerLedgeDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.SearchState);
        }
        else if (_playerDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }

        //TODO: if player is in melee range, do attack

        if (StateDuration >= _data.ChargeDuration)
        {
            // if player still detected, do it again
            if (_playerDetector.IsDetected)
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
