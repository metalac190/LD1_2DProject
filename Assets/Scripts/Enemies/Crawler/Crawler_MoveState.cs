using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_MoveState : State
{
    private CrawlerFSM _stateMachine;
    private Crawler _crawler;
    private CrawlerData _data;

    private EnvironmentDetector _environmentDetector;

    public Crawler_MoveState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
        _data = crawler.Data;

        _environmentDetector = crawler.EnvironmentDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _crawler.Move(_data.MovementSpeed);

        _environmentDetector.StartCheckingEnvironment();
    }

    public override void Exit()
    {
        base.Exit();

        _environmentDetector.StopCheckingEnvironment();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if we've reached the path end
        if (_environmentDetector.IsWallDetected || _environmentDetector.IsLedgeDetected)
        {
            // turn around
            _crawler.Flip();
            _crawler.Move(_data.MovementSpeed);
        }
    }

    public override void Update()
    {
        base.Update();

    }
}
