using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_IdleState : State
{
    private float _idleTime = 0;

    private bool _isPlayerInMinAggroRange = false;

    CrawlerFSM _stateMachine;
    Crawler _crawler;
    PlayerDetector _playerDetector;

    public Crawler_IdleState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
        _playerDetector = crawler.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _crawler.SetVelocity(0);
        SetRandomIdleTime();

        _playerDetector.PlayerDetected.AddListener(OnPlayerDetected);
        //_isPlayerInMinAggroRange = _crawler.CheckPlayerInMinAggroRange();
    }

    public override void Exit()
    {
        base.Exit();

        _playerDetector.PlayerDetected.RemoveListener(OnPlayerDetected);

        if (_crawler.CheckLedge() || _crawler.CheckWall())
        {
            _crawler.Flip();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //_isPlayerInMinAggroRange = _crawler.CheckPlayerInMinAggroRange();
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

    private void OnPlayerDetected()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
    }
}
