using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerChasingState : State
{
    private FlyerFSM _stateMachine;
    private Flyer _flyer;

    private OverlapDetector _playerInRange;
    private MovementKM _movement;

    private Transform _objectToChase;

    public FlyerChasingState(FlyerFSM stateMachine, Flyer flyer)
    {
        _stateMachine = stateMachine;
        _flyer = flyer;

        _playerInRange = flyer.PlayerDetector.PlayerInRange;
        _movement = flyer.Movement;
    }

    public override void Enter()
    {
        base.Enter();

        _objectToChase = _playerInRange.LastDetectedCollider.transform;

        _playerInRange.StartDetecting();
    }

    public override void Exit()
    {
        base.Exit();

        _playerInRange.StopDetecting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if there is an object to chase, chase it
        if(_playerInRange.IsDetected && _objectToChase != null)
        {
            Vector2 direction = (_objectToChase.position
                - _flyer.transform.position).normalized;
            _movement.Move(direction * _flyer.ChaseSpeed, true);
        }
        // otherwise we lost the object
        else
        {
            _stateMachine.ChangeState(_stateMachine.ReturningState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("Chase object: " + _objectToChase.gameObject.name);
    }
}
