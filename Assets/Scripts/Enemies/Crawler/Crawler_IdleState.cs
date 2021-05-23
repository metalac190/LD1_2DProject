using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_IdleState : State
{
    private float _idleTime = 0;

    CrawlerFSM _stateMachine;
    Crawler _crawler;

    public Crawler_IdleState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
    }

    public override void Enter()
    {
        base.Enter();

        _crawler.SetVelocity(0);
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (_crawler.CheckLedge() || _crawler.CheckWall())
        {
            _crawler.Flip();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(StateDuration >= _idleTime)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }

    private void SetRandomIdleTime()
    {
        _idleTime = UnityEngine.Random.Range(_crawler.MinIdleTime, _crawler.MaxIdleTime);
    }
}
