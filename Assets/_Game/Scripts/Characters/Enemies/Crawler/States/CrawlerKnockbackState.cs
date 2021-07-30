using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerKnockbackState : State
{
    private CrawlerFSM _stateMachine;

    private ReceiveKnockback _receiveKnockback;
    private DamageZone _damageZone;

    public CrawlerKnockbackState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;

        _receiveKnockback = crawler.ReceiveKnockback;
        _damageZone = crawler.DamageZone;
    }

    public override void Enter()
    {
        base.Enter();

        _receiveKnockback.KnockbackEnded += OnKnockbackEnded;
        _damageZone.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveKnockback.KnockbackEnded -= OnKnockbackEnded;
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

    private void OnKnockbackEnded()
    {
        _stateMachine.ChangeState(_stateMachine.MoveState);
    }
}
