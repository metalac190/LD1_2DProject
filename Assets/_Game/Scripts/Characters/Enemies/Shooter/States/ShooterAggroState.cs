using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAggroState : State
{
    private ShooterFSM _stateMachine;
    private Shooter _shooter;

    private ColliderDetector _playerInRange;
    private float _timeSinceLastShot;
    private float _shootRate;

    public ShooterAggroState(ShooterFSM stateMachine, Shooter shooter)
    {
        _stateMachine = stateMachine;
        _shooter = shooter;

        _playerInRange = shooter.PlayerDetector.PlayerInRange;
        _shootRate = shooter.ShootRate;
    }

    public override void Enter()
    {
        base.Enter();

        _timeSinceLastShot = 0;

        _playerInRange.Detect();
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
    }

    public override void Update()
    {
        base.Update();

        CheckPlayerInRange();
        ProgressFireCounter();
    }


    private void CheckPlayerInRange()
    {
        if (_playerInRange.LastDetectedCollider == null || _playerInRange.IsDetected == false)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }

    private void ProgressFireCounter()
    {
        _timeSinceLastShot += Time.deltaTime;

        if (_timeSinceLastShot >= _shootRate)
        {
            _shooter.Shoot(_playerInRange.LastDetectedCollider.transform);
            _timeSinceLastShot = 0;
        }
    }


}
