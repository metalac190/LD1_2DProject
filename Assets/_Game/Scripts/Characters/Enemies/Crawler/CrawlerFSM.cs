using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Crawler))]
public class CrawlerFSM : StateMachineMB
{
    public CrawlerMoveState MoveState { get; private set; }
    public CrawlerKnockbackState KnockbackState { get; private set; }

    [SerializeField] 
    private Crawler _crawler;

    private ReceiveKnockback _receiveKnockback;

    private void Awake()
    {
        // create states
        MoveState = new CrawlerMoveState(this, _crawler);
        KnockbackState = new CrawlerKnockbackState(this, _crawler);
        // any state transitions
        _receiveKnockback = _crawler.ReceiveKnockback;
    }

    protected override void OnEnable()
    {
        _receiveKnockback.KnockbackStarted += OnKnockbackStarted;
    }

    protected override void OnDisable()
    {
        _receiveKnockback.KnockbackStarted -= OnKnockbackStarted;

    }

    private void Start()
    {
        ChangeState(MoveState);
    }

    private void OnKnockbackStarted()
    {
        ChangeState(KnockbackState);
    }
}
