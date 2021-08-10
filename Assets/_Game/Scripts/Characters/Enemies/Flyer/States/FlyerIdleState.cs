using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerIdleState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private OverlapDetector _playerInRange;

    public FlyerIdleState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _playerInRange = flyer.PlayerDetector.PlayerInRange;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("FLYER: Idle State");

        _playerInRange.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerInRange.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (_playerInRange.IsDetected)
        {
            _stateMachine.ChangeState(_stateMachine.ChasingState);
            return;
        }
    }
}
