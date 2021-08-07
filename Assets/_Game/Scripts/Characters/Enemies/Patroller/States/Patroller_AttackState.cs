using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_AttackState : State
{
    private PatrollerFSM _stateMachine;
    private Patroller _patroller;
    private PatrollerData _data;

    private MovementKM _movement;
    private RayDetector _aggroDetector;
    private HitVolume _hitVolume;
    private GameObject _detectedGraphic;


    public bool IsAttackActive { get; private set; }
    private bool _isAttackSequenceComplete;

    Coroutine _attackRoutine;

    public Patroller_AttackState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _movement = patroller.Movement;
        _aggroDetector = patroller.AggroDetector;
        _hitVolume = patroller.HitVolume;
        _detectedGraphic = patroller.DetectedGraphic;
    }

    public override void Enter()
    {
        base.Enter();

        _detectedGraphic.SetActive(true);
        _movement.MoveX(0, true);

        // wait for startup time before triggering the attack
        IsAttackActive = false;
        _isAttackSequenceComplete = false;

        if (_attackRoutine != null)
            _stateMachine.StopCoroutine(_attackRoutine);
        _attackRoutine = _stateMachine.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        base.Exit();

        _detectedGraphic.SetActive(false);
        _hitVolume.gameObject.SetActive(false);

        IsAttackActive = false;
        _isAttackSequenceComplete = false;
        // stop routine if it's active and we're exiting early
        if (_attackRoutine != null)
        {
            _stateMachine.StopCoroutine(_attackRoutine);
        }
            
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();


    }

    public override void Update()
    {
        base.Update();

        // if our attack is active and has exceeded the attack duration
        if (_isAttackSequenceComplete)
        {
            if (_aggroDetector.Detect() != null)
            {
                _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
                return;
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.SearchState);
                return;
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_data.AttackStartupDuration);

        TriggerAttack();

        yield return new WaitForSeconds(_data.AttackActiveDuration);

        FinishAttack();

        yield return new WaitForSeconds(_data.AttackAfterDuration);

        _isAttackSequenceComplete = true;
    }

    private void TriggerAttack()
    {
        IsAttackActive = true;
        _hitVolume.gameObject.SetActive(true);
    }

    private void FinishAttack()
    {
        IsAttackActive = false;
        _hitVolume.gameObject.SetActive(false);
    }
}
