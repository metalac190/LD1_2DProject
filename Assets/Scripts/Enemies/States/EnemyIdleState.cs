using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    protected EnemyIdleStateData StateData;
    protected bool FlipAfterIdle;
    protected bool IsIdleTimeOver;
    protected float IdleTime;

    public EnemyIdleState(Entity entity, FiniteStateMachine stateMachine, 
        string animBoolName, EnemyIdleStateData stateData) 
        : base(entity, stateMachine, animBoolName)
    {
        this.StateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Entity.SetVelocity(0);
        IsIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (FlipAfterIdle)
        {
            Entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime)
        {
            IsIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        FlipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        IdleTime = UnityEngine.Random.Range(StateData.MinIdleTime, StateData.MaxIdleTime);
    }
}
