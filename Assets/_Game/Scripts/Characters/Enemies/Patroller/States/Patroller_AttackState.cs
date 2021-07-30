using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller_AttackState : State
{
    PatrollerFSM _stateMachine;
    Patroller _patroller;
    PatrollerData _data;

    PlayerDetector _playerDetector;

    public bool IsAttackActive { get; private set; }
    private bool _isAttackSequenceComplete;

    Coroutine _attackRoutine;
    GameObject _detectedGraphic;

    public Patroller_AttackState(PatrollerFSM stateMachine, Patroller patroller)
    {
        _stateMachine = stateMachine;
        _patroller = patroller;
        _data = patroller.Data;

        _playerDetector = patroller.PlayerDetector;
        _detectedGraphic = patroller.DetectedGraphic;
    }

    public override void Enter()
    {
        base.Enter();

        _detectedGraphic.SetActive(true);
        _patroller.Move(0);
        // adjust visual graphic - multiply x2 to convert radius to scale units
        _patroller.AttackLocation.transform.localScale 
            = new Vector2(_data.AttackRadius * 2, _data.AttackRadius * 2);

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

        IsAttackActive = false;
        _isAttackSequenceComplete = false;
        // stop routine if it's active and we're exiting early
        if (_attackRoutine != null)
        {
            _stateMachine.StopCoroutine(_attackRoutine);
            _patroller.AttackLocation.SetActive(false);
        }
            
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if our attack is active and has exceeded the attack duration
        if(_isAttackSequenceComplete)
        {
            if (_playerDetector.CheckPlayerInAggroRange())
            {
                _stateMachine.ChangeState(_stateMachine.PlayerDetectedState);
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.SearchState);
            }
        }
    }

    public override void Update()
    {
        base.Update();
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
        _patroller.AttackLocation.SetActive(true);

        //TODO: replace the below for an extended hitbox later on
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(
            _patroller.AttackLocation.transform.position, _data.AttackRadius, _data.AttackableLayers);
        Health health;
        foreach(Collider2D collider in detectedObjects)
        {
            health = collider.GetComponent<Health>();
            health?.Damage(_data.AttackDamage);
        }
    }

    private void FinishAttack()
    {
        IsAttackActive = false;
        _patroller.AttackLocation.SetActive(false);
    }
}
