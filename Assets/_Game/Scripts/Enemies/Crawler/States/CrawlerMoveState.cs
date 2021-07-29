using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerMoveState : State
{
    private CrawlerFSM _stateMachine;
    private CrawlerData _data;

    private MovementKM _kinematicObject;
    private WallDetector _wallDetector;
    private LedgeDetector _ledgeDetector;

    public CrawlerMoveState(CrawlerFSM stateMachine, Crawler crawler)
    {
        _stateMachine = stateMachine;
        _data = crawler.Data;

        _kinematicObject = crawler.Movement;
        _wallDetector = crawler.WallDetector;
        _ledgeDetector = crawler.LedgeDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Crawler Move");

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _wallDetector.DetectWall();
        _ledgeDetector.DetectLowerLedge();

        // if we've reached the path end
        if (_wallDetector.IsWallDetected || _ledgeDetector.IsDetectingLowerLedge)
        {
            // turn around
            _kinematicObject.Flip();
            _kinematicObject.MoveX(_data.MovementSpeed * _kinematicObject.FacingDirection, true);
        }
        else
        {
            _kinematicObject.MoveX(_data.MovementSpeed * _kinematicObject.FacingDirection, true);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
