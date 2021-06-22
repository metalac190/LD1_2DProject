using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_StunState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;

    ReceivePushForce _receiveKnockback;
    PlayerDetector _playerDetector;

    public Patroller_StunState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;

        _receiveKnockback = patroller.ReceiveKnockback;
        _playerDetector = patroller.PlayerDetector;
    }

    public override void Enter()
    {
        base.Enter();

        _receiveKnockback.KnockbackEnded += OnKnockbackEnded;

        Debug.Log("STATE: Knockback!");
    }

    public override void Exit()
    {
        base.Exit();

        _receiveKnockback.KnockbackEnded -= OnKnockbackEnded;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnKnockbackEnded()
    {
        _patroller.Move(0);

        _stateMachine.ChangeState(_stateMachine.SearchState);
    }
}
