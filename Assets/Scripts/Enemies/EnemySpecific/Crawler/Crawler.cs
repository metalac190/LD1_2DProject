using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Entity
{
    public Crawler_IdleState IdleState { get; private set; }
    public Crawler_MoveState MoveState { get; private set; }

    [SerializeField] 
    private EnemyIdleStateData _idleStateData;
    [SerializeField]
    private EnemyMoveStateData _moveStateData;


    public override void Awake()
    {
        base.Awake();

        MoveState = new Crawler_MoveState(this, StateMachine, "Move", _moveStateData, this);
        IdleState = new Crawler_IdleState(this, StateMachine, "Idle", _idleStateData, this);

        StateMachine.Initialize(IdleState);
    }
}
