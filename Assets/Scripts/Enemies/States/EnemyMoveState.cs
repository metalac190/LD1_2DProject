using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
    protected EnemyMoveStateData StateData;
    protected bool IsDetectingWall;
    protected bool IsDetectingLedge;

    public EnemyMoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName,
        EnemyMoveStateData stateData)
        : base(entity, stateMachine, animBoolName)
    {
        this.StateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        Entity.SetVelocity(StateData.MovementSpeed);

        IsDetectingLedge = Entity.CheckLedge();
        IsDetectingWall = Entity.CheckWall();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        IsDetectingLedge = Entity.CheckLedge();
        IsDetectingWall = Entity.CheckWall();
    }
}
