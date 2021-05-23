using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : StateMachineMB
{
    public Crawler_IdleState IdleState { get; private set; }
    public Crawler_MoveState MoveState { get; private set; }

    private Crawler _crawler;

    private void Awake()
    {
        _crawler = GetComponent<Crawler>();
        // create states
        IdleState = new Crawler_IdleState(this, _crawler);
        MoveState = new Crawler_MoveState(this, _crawler);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }
}
