using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack 01")]
    [SerializeField] 
    private bool _combatEnabled = true;
    [SerializeField] 
    private float _inputTimer = .1f;  // how much buffer will we allow input
    [SerializeField]
    private float _attack01Radius;
    [SerializeField]
    private int _attack01Damage;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _attack01HitBox;
    [SerializeField]
    private float _attack01Duration;
    [SerializeField]
    private GameObject _attack01Visual;

    private bool _canAttack = true;
    private bool _receivedInput;
    private bool _isAttacking;
    private bool _isFirstAttack;

    private float _lastInputTime = Mathf.NegativeInfinity;

    private Coroutine _attack01Routine;

    private void Awake()
    {
        _animator.SetBool("CombatEnabled", _combatEnabled);
        // if we're using an attack visual, make sure it's disabled
        _attack01Visual.SetActive(false);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttack();
    }

    private void CheckCombatInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_combatEnabled)
            {
                // attempt combat
                _receivedInput = true;
                _lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttack()
    {
        // perform attack1
        if (_receivedInput)
        {
            // if we're not already attacking, attack!
            if (!_isAttacking)
            {
                Debug.Log("Attack!");

                _receivedInput = false;
                _isAttacking = true;
                // alternate attack visuals
                _isFirstAttack = !_isFirstAttack;
                // start sequence
                if (_attack01Routine != null)
                    StopCoroutine(_attack01Routine);
                _attack01Routine = StartCoroutine(Attack01Routine(_attack01Duration));
                // update animations
                _animator.SetBool("Attack01", true);
                _animator.SetBool("FirstAttack", _isFirstAttack);
                _animator.SetBool("IsAttacking", _isAttacking);
            }
        }
        // open buffer for early input
        if(Time.time >= _lastInputTime + _inputTimer)
        {
            _receivedInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll
            (_attack01HitBox.position, _attack01Radius);
        foreach(Collider2D collider in detectedObjects)
        {
            collider.GetComponent<Health>()?.TakeDamage(_attack01Damage);
            // instantiate hit particle
        }
    }

    private void FinishAttack01()
    {
        _isAttacking = false;
        _animator.SetBool("IsAttacking", _isAttacking);
        _animator.SetBool("Attack01", false);
    }

    private IEnumerator Attack01Routine(float duration)
    {
        
        //TODO just use player sprite if desired, this is optional
        _attack01Visual.SetActive(true);

        yield return new WaitForSeconds(duration);

        _attack01Visual.SetActive(false);
        // disable weapon sprite
        FinishAttack01();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attack01HitBox.position, _attack01Radius);
    }
}
