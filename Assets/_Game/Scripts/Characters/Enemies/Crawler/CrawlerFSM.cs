using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : StateMachineMB
{
    public CrawlerMoveState MoveState { get; private set; }
    public CrawlerHitState HitState { get; private set; }

    [SerializeField] 
    private Crawler _crawler;

    private ReceiveHit _receiveHit;

    private void Awake()
    {
        // create states
        MoveState = new CrawlerMoveState(this, _crawler);
        HitState = new CrawlerHitState(this, _crawler);
        // any state transitions
        _receiveHit = _crawler.ReceiveHit;
    }

    protected override void OnEnable()
    {
        _receiveHit.HitReceived.AddListener(OnHitReceived);
    }

    protected override void OnDisable()
    {
        _receiveHit.HitReceived.RemoveListener(OnHitReceived);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }

    private void OnHitReceived()
    {
        ChangeState(HitState);
    }
}
