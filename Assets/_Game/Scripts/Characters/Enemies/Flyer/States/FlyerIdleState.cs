using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerIdleState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private OverlapDetector _playerDetector;

    public FlyerIdleState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _playerDetector = flyer.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("FLYER: Idle State");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (_playerDetector.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.ChasingState);
        }
    }
}
