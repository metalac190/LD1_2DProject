using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_HitState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;

    private MovementKM _movement;
    private RayDetector _playerLOS;
    private ReceiveHit _receiveHit;

    public Patroller_HitState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;

        _movement = patroller.Movement;
        _playerLOS = patroller.PlayerDetector.PlayerLOS;
        _receiveHit = patroller.ReceiveHit;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Knockback!");

        _receiveHit.HitRecovered += OnHitRecovered;
    }

    public override void Exit()
    {
        base.Exit();

        _receiveHit.HitRecovered -= OnHitRecovered;

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnHitRecovered()
    {
        _movement.MoveX(0, false);
        _stateMachine.ChangeState(_stateMachine.SearchState);
    }
}
