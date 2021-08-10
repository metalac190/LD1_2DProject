using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerMoveState : State
{
    private CrawlerFSM _stateMachine;
    private Crawler _crawler;

    private MovementKM _kinematicObject;
    private OverlapDetector _wallDetector;
    private OverlapDetector _groundDetector;
    private OverlapDetector _groundInFrontDetector;

    public CrawlerMoveState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _crawler = crawler;

        _kinematicObject = crawler.Movement;
        _wallDetector = crawler.EnvironmentDetector.WallDetector;
        _groundDetector = crawler.EnvironmentDetector.GroundDetector;
        _groundInFrontDetector = crawler.EnvironmentDetector.GroundInFrontDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _groundInFrontDetector.StartDetecting();
        _wallDetector.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _groundInFrontDetector.StopDetecting();
        _wallDetector.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if we've reached a wall
        if (_wallDetector.IsDetected)
        {
            // turn around
            TurnAround();
            // detect again in new direction to reset
            _wallDetector.Detect();
        }
        // or a ledge
        else if(!_groundInFrontDetector.IsDetected)
        {
            // if there's no ground in front, but ground beneath, it's a ledge
            if (_groundDetector.Detect() != null)
            {
                TurnAround();
                // detect in new direction to reset
                _groundInFrontDetector.Detect();
            }
        }
        // otherwise, keep moving
        else
        {
            _kinematicObject.MoveX(_crawler.MovementSpeed * _kinematicObject.FacingDirection, true);
        }
    }

    private void TurnAround()
    {
        _kinematicObject.Flip();
        _kinematicObject.MoveX(_crawler.MovementSpeed
            * _kinematicObject.FacingDirection, true);
    }

    public override void Update()
    {
        base.Update();
    }
}
