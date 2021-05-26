using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerFSM : StateMachineMB
{
    public Patroller_IdleState IdleState { get; private set; }
    public Patroller_MoveState MoveState { get; private set; }
    public Patroller_PlayerDetectedState PlayerDetectedState { get; private set; }
    public Patroller_ChargeState ChargeState { get; private set; }
    public Patroller_SearchState SearchState { get; private set; }
    public Patroller_AttackState AttackState { get; private set; }

    [SerializeField]
    private Patroller _patroller;

    private void Awake()
    {
        // create states
        IdleState = new Patroller_IdleState(this, _patroller);
        MoveState = new Patroller_MoveState(this, _patroller);
        PlayerDetectedState = new Patroller_PlayerDetectedState(this, _patroller);
        ChargeState = new Patroller_ChargeState(this, _patroller);
        SearchState = new Patroller_SearchState(this, _patroller);
        AttackState = new Patroller_AttackState(this, _patroller);
    }

    private void Start()
    {
        ChangeState(MoveState);
    }
}
