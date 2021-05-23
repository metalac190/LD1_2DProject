using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_PlayerDetectedState : State
{
    private bool _isPlayerInMinAggroRange = false;
    private bool _isPlayerInMaxAggroRange = false;

    private CrawlerFSM _stateMachine;
    private Crawler _crawler;
    private PlayerDetector _playerDetector;

    public Crawler_PlayerDetectedState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;
        _playerDetector = crawler.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _crawler.SetVelocity(0);

        _playerDetector.PlayerEscaped.AddListener(OnPlayerEscaped);
        //CheckForPlayer();
    }

    public override void Exit()
    {
        base.Exit();

        _playerDetector.PlayerEscaped.RemoveListener(OnPlayerEscaped);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //CheckForPlayer();
    }

    public override void Update()
    {
        base.Update();
    }

    /*
    private void CheckForPlayer()
    {
        _isPlayerInMinAggroRange = _playerDetector.CheckPlayerInMinAggroRange();
        _isPlayerInMaxAggroRange = _playerDetector.CheckPlayerInMaxAggroRange();
    }
    */

    private void OnPlayerEscaped()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
