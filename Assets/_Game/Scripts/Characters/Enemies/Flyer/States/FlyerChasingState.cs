using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerChasingState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private ObjectDetector _objectDetector;
    private MovementKM _movement;
    private Transform _objectToChase;

    public FlyerChasingState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _objectDetector = flyer.ObjectDetector;
        _movement = flyer.Movement;
    }

    public override void Enter()
    {
        base.Enter();

        _objectToChase = _objectDetector.DetectedObjects[0].transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if there is an object to chase, move
        if(_objectDetector.IsObjectDetected && _objectToChase != null)
        {
            Vector2 direction = (_objectToChase.position
                - _flyer.transform.position).normalized;
            _movement.Move(direction * _flyer.ChaseSpeed, true);
        }
        // otherwise we lost the object
        else
        {
            _stateMachine.ChangeState(_stateMachine.ReturningState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
