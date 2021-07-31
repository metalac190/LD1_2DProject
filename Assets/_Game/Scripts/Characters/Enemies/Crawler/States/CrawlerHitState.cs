using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHitState : State
{
    private CrawlerFSM _stateMachine;

    private DamageZone _damageZone;
    private ReceiveHit _receiveHit;

    public CrawlerHitState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;

        _receiveHit = crawler.ReceiveHit;
        _damageZone = crawler.DamageZone;
    }

    public override void Enter()
    {
        base.Enter();

        _receiveHit.HitRecovered += OnHitRecovered;
        _damageZone.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveHit.HitRecovered -= OnHitRecovered;
        _damageZone.enabled = true;
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
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }
}
