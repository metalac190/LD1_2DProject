using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerHitState : State
{
    private FlyerFSM _stateMachine;

    private HitVolume _hitVolume;
    private ReceiveHit _receiveHit;

    public FlyerHitState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;

        _receiveHit = flyer.ReceiveHit;
        _hitVolume = flyer.HitVolume;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("FLYER: Hit State");
        _receiveHit.HitRecovered += OnHitRecovered;
        _hitVolume.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveHit.HitRecovered -= OnHitRecovered;
        _hitVolume.enabled = true;
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
