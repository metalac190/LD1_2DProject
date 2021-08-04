using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerIdleState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private ObjectDetector _objectDetector;

    public FlyerIdleState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _objectDetector = flyer.ObjectDetector;
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

        if (_objectDetector.IsObjectDetected)
        {
            _stateMachine.ChangeState(_stateMachine.ChasingState);
        }
    }
}
