using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flyer))]
public class FlyerFSM : StateMachineMB
{
    private Flyer _flyer;

    public FlyerIdleState IdleState { get; private set; }
    public FlyerChasingState ChasingState { get; private set; }
    public FlyerReturningState ReturningState { get; private set; }
    public FlyerHitState HitState { get; private set; }

    private ReceiveHit _receiveHit;

    private void Awake()
    {
        _flyer = GetComponent<Flyer>();
        _receiveHit = _flyer.ReceiveHit;
        // states
        IdleState = new FlyerIdleState(this, _flyer);
        ChasingState = new FlyerChasingState(this, _flyer);
        ReturningState = new FlyerReturningState(this, _flyer);
        HitState = new FlyerHitState(this, _flyer);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _receiveHit.HitReceived.AddListener(OnHitReceived);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _receiveHit.HitReceived.RemoveListener(OnHitReceived);

    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void OnHitReceived()
    {
        ChangeState(HitState);
    }
}
