using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_MoveState : EnemyMoveState
{
    private Crawler _crawler;

    public Crawler_MoveState(Entity entity, FiniteStateMachine stateMachine, 
        string animBoolName, EnemyMoveStateData stateData, Crawler crawler) 
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this._crawler = crawler;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!IsDetectingWall || !IsDetectingLedge)
        {
            _crawler.IdleState.SetFlipAfterIdle(true);
            StateMachine.ChangeState(_crawler.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
