using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : StateMachineMB
{
    public Crawler_IdleState IdleState { get; private set; }
    public Crawler_MoveState MoveState { get; private set; }
    public Crawler_PlayerDetectedState PlayerDetectedState { get; private set; }

    [SerializeField] 
    private Crawler _crawler;

    private void Awake()
    {
        // create states
        IdleState = new Crawler_IdleState(this, _crawler);
        MoveState = new Crawler_MoveState(this, _crawler);
        PlayerDetectedState = new Crawler_PlayerDetectedState(this, _crawler);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }
}
