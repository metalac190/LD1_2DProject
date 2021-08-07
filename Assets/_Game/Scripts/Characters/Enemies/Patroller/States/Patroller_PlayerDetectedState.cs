using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_PlayerDetectedState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private RayDetector _aggroDetector;
    private OverlapDetector _closeRangeDetector;
    private GameObject _detectedGraphic;    // graphic icon that communicates detect state

    public Patroller_PlayerDetectedState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _aggroDetector = patroller.AggroDetector;
        _closeRangeDetector = patroller.CloseRangeDetector;
        _detectedGraphic = patroller.DetectedGraphic;
    }

    public override void Enter()
    {
        base.Enter();

        _detectedGraphic.SetActive(true);

        _aggroDetector.StartDetecting();
        _closeRangeDetector.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _detectedGraphic.SetActive(false);

        _aggroDetector.StopDetecting();
        _closeRangeDetector.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // pause briefly as if surprised
        if(StateDuration >= _data.DetectedPauseTime)
        {
            // if player is in close range, do an attack
            if (_closeRangeDetector.IsDetected)
            {
                _stateMachine.ChangeState(_stateMachine.AttackState);
                return;
            }
            // otherwise if player is still detected, charge
            else if (_aggroDetector.IsDetected)
            {
                _stateMachine.ChangeState(_stateMachine.ChargeState);
                return;
            }
            // if player is gone, go back to patrol
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
