using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardentCrawler_IdleState : EnemyIdleState
{
    private BardentCrawler _crawler;

    public BardentCrawler_IdleState(Entity entity, BardentFSM stateMachine, 
        string animBoolName, EnemyIdleStateData stateData, BardentCrawler crawler) 
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

        if (IsIdleTimeOver)
        {
            StateMachine.ChangeState(_crawler.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
