using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterIdleState : State
{
    private ShooterFSM _stateMachine;

    private ColliderDetector _playerInRange;

    public ShooterIdleState(ShooterFSM stateMachine, Shooter shooter)
    {
        _stateMachine = stateMachine;

        _playerInRange = shooter.PlayerDetector.PlayerInRange;
    }

    public override void Enter()
    {
        base.Enter();

        _playerInRange.Detect();

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
            _stateMachine.ChangeState(_stateMachine.AggroState);
            return;
        }
    }
}
