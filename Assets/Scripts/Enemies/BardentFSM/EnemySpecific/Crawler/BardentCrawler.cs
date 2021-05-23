using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardentCrawler : Entity
{
    public BardentCrawler_IdleState IdleState { get; private set; }
    public BardentCrawler_MoveState MoveState { get; private set; }

    [SerializeField] 
    private EnemyIdleStateData _idleStateData;
    [SerializeField]
    private EnemyMoveStateData _moveStateData;


    public override void Awake()
    {
        base.Awake();

        MoveState = new BardentCrawler_MoveState(this, StateMachine, "Move", _moveStateData, this);
        IdleState = new BardentCrawler_IdleState(this, StateMachine, "Idle", _idleStateData, this);

        StateMachine.Initialize(IdleState);
    }
}
