using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_MoveState : State
{
    private CrawlerFSM _stateMachine;
    private Crawler _crawler;
    private PlayerDetector _playerDetector;

    private bool _isDetectingWall = false;
    private bool _isDetectingLedge = false;
    private bool _isPlayerInMinAggroRange = false;

    public Crawler_MoveState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
        _playerDetector = crawler.PlayerDetector;
    }

    public override void Enter()
    {
        _crawler.SetVelocity(_crawler.MovementSpeed);

        CheckEnvironment();

        _playerDetector.PlayerDetected.AddListener(OnPlayerDetected);
        //_isPlayerInMinAggroRange = _playerDetector.CheckPlayerInMinAggroRange();
    }

    private void CheckEnvironment()
    {
        _isDetectingLedge = _crawler.CheckLedge();
        _isDetectingWall = _crawler.CheckWall();
    }

    public override void Exit()
    {
        base.Exit();

        _playerDetector.PlayerDetected.RemoveListener(OnPlayerDetected);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckEnvironment();

        //_isPlayerInMinAggroRange = _playerDetector.CheckPlayerInMinAggroRange();
    }

    public override void Update()
    {
        base.Update();
        // if we've reached the path end
        if(_isDetectingWall || _isDetectingLedge)
        {
            // idle if specified
            if (_crawler.IdleOnPathEnd)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
            // otherwise turn and continue
            else
            {
                _crawler.Flip();
                _crawler.SetVelocity(_crawler.MovementSpeed);
            }
        }

    }

    private void OnPlayerDetected()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
    }
}
