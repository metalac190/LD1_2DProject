using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler_IdleState : EnemyIdleState
{
    private Crawler _crawler;

    public Crawler_IdleState(Entity entity, FiniteStateMachine stateMachine, 
        string animBoolName, EnemyIdleStateData stateData, Crawler crawler) 
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
