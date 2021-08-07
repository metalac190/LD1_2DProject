using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerReturningState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private MovementKM _movement;

    public FlyerReturningState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _movement = flyer.Movement;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // move back towards start
        Vector2 direction = (_flyer.StartPosition
            - _flyer.transform.position).normalized;
        _movement.Move(direction * _flyer.ReturnSpeed, true);

        // check if we've returned to start
        if (Vector2.Distance(_flyer.StartPosition, _flyer.transform.position)
            <= .1f)
        {
            // we've returned to our starting point
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
