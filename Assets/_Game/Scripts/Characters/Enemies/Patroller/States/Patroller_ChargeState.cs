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
    private ColliderDetector _groundInFrontDetector;
    private ColliderDetector _closeRangeDetector;
    private RayDetector _aggroDetector;
    GameObject _detectedGraphic;

    public Patroller_ChargeState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _groundDetector = patroller.GroundDetector;
        _wallDetector = patroller.WallDetector;
        _groundInFrontDetector = patroller.GroundInFrontDetector;
        _closeRangeDetector = patroller.CloseRangeDetector;
        _aggroDetector = patroller.AggroDetector;
        _detectedGraphic = patroller.DetectedGraphic;
    }

    public override void Enter()
    {
        base.Enter();

        _wallDetector.StartDetecting();
        _groundInFrontDetector.StartDetecting();
        _aggroDetector.StartDetecting();
        _closeRangeDetector.StartDetecting();

        _detectedGraphic.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        _wallDetector.StopDetecting();
        _groundInFrontDetector.StopDetecting();
        _aggroDetector.StopDetecting();
        _closeRangeDetector.StopDetecting();

        _detectedGraphic.SetActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _movement.MoveX(_data.ChargeSpeed * _movement.FacingDirection, true);

        // if there's a wall, go back to searching
        if (_wallDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.SearchState);
            return;
        }
        // or if there's a ledge, go back to searching
        else if(_groundInFrontDetector.IsDetected == false)
        {
            // if there's no ground in front, but we're grounded, it's a ledge
            if(_groundDetector.Detect() != null)
            {
                _stateMachine.ChangeState(_stateMachine.SearchState);
                return;
            }
        }
        // or if the player is in close range, attack!
        else if (_closeRangeDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
            return;
        }

        // if we've been charging too long, take a differnt action
        if (StateDuration >= _data.ChargeDuration)
        {
            // if player still detected, do it again
            if (_aggroDetector.IsDetected)
            {
                _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
                return;
            }
            // otherwise transition back to patrol
            else
            {
                _stateMachine.ChangeState(_stateMachine.SearchState);
                return;
            }
        }
    }

    public override void Update()
    {
        base.Update();

    }
}
