using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : StateMachineMB
{
    public Crawler_MoveState MoveState { get; private set; }

    [SerializeField] 
    private Crawler _crawler;

    private void Awake()
    {
        // create states
        MoveState = new Crawler_MoveState(this, _crawler);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }
}
