using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardentState
{
    protected BardentFSM StateMachine;
    protected Entity Entity;

    protected float StartTime;
    protected string AnimBoolName;

    public BardentState(Entity entity, BardentFSM stateMachine, string animBoolName)
    {
        this.Entity = entity;
        this.StateMachine = stateMachine;
        this.AnimBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        StartTime = Time.time;
        Entity.Anim.SetBool(AnimBoolName, true);
    }

    public virtual void Exit()
    {
        Entity.Anim.SetBool(AnimBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
}
