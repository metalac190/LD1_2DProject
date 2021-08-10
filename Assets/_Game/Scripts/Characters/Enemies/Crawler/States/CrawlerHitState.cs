using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHitState : State
{
    private CrawlerFSM _stateMachine;

    private HitVolume _hitVolume;
    private ReceiveHit _receiveHit;

    public CrawlerHitState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;

        _receiveHit = crawler.ReceiveHit;
        _hitVolume = crawler.HitVolume;
    }

    public override void Enter()
    {
        base.Enter();

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
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }
}
