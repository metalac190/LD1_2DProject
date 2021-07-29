using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : EnemyFSM
{
    public CrawlerMoveState MoveState { get; private set; }

    [SerializeField] 
    private Crawler _crawler;

    protected override void Awake()
    {
        base.Awake();
        // create states
        MoveState = new CrawlerMoveState(this, _crawler);
    }

    protected override void Start()
    {
        ChangeState(MoveState);
    }
}
