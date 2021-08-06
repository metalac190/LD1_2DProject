using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Patroller))]
public class PatrollerFSM : StateMachineMB
{
    public Patroller_IdleState IdleState { get; private set; }
    public Patroller_MoveState MoveState { get; private set; }
    public Patroller_PlayerDetectedState PlayerDetectedState { get; private set; }
    public Patroller_ChargeState ChargeState { get; private set; }
    public Patroller_SearchState SearchState { get; private set; }
    public Patroller_AttackState AttackState { get; private set; }
    public Patroller_StunState StunState { get; private set; }
    public Patroller_DeadState DeadState { get; private set; }

    private Patroller _patroller;

    private Health _health;
    private ReceiveHit _receiveHit;

    private void Awake()
    {
        _patroller = GetComponent<Patroller>();
        _health = _patroller.Health;
        _receiveHit = _patroller.ReceiveHit;
        // create states
        IdleState = new Patroller_IdleState(this, _patroller);
        MoveState = new Patroller_MoveState(this, _patroller);
        PlayerDetectedState = new Patroller_PlayerDetectedState(this, _patroller);
        ChargeState = new Patroller_ChargeState(this, _patroller);
        SearchState = new Patroller_SearchState(this, _patroller);
        AttackState = new Patroller_AttackState(this, _patroller);
        StunState = new Patroller_StunState(this, _patroller);
        DeadState = new Patroller_DeadState(this, _patroller);
    }

    protected override void OnEnable()
    {
        // any-state events
        //_patroller.ReceiveKnockback.KnockbackStarted += OnKnockbackStarted;
        _patroller.Health.Died.AddListener(OnDied);
    }

    protected override void OnDisable()
    {
        //_patroller.ReceiveKnockback.KnockbackStarted -= OnKnockbackStarted;
        _patroller.Health.Died.RemoveListener(OnDied);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }

    private void OnDied()
    {
        Debug.Log("On Died!");
        ChangeState(DeadState);
    }
}
