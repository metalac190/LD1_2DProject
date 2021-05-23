using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_MoveState : State
{
    private CrawlerFSM _stateMachine;
    private Crawler _crawler;

    private bool _isDetectingWall = false;
    private bool _isDetectingLedge = false;

    public Crawler_MoveState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
    }

    public override void Enter()
    {
        _crawler.SetVelocity(_crawler.MovementSpeed);

        _isDetectingLedge = _crawler.CheckLedge();
        _isDetectingWall = _crawler.CheckWall();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _isDetectingLedge = _crawler.CheckLedge();
        _isDetectingWall = _crawler.CheckWall();
    }

    public override void Update()
    {
        base.Update();

        if(_isDetectingWall || _isDetectingLedge)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
