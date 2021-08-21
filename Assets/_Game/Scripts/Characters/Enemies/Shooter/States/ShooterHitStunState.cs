using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHitStunState : State
{
    private ShooterFSM _stateMachine;

    private ReceiveHit _receiveHit;
    private float _stunTime;

    public ShooterHitStunState(ShooterFSM stateMachine, Shooter shooter)
    {
        _stateMachine = stateMachine;

        _receiveHit = shooter.ReceiveHit;
    }

    public override void Enter()
    {
        base.Enter();

        _receiveHit.HitRecovered += OnHitRecovered;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveHit.HitRecovered -= OnHitRecovered;

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnHitRecovered()
    {
        _stateMachine.ChangeStateToPrevious();
    }
}
