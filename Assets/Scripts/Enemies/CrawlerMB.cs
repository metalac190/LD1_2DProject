using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerMB : MonoBehaviour
{
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }

    private State _currentState = State.Moving;
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 3;

    [Header("Environment Detection")]
    [SerializeField] private float _groundCheckDistance = .5f;
    [SerializeField] private float _wallCheckDistance = .2f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _whatIsGround;

    private int _facingDirection = 1;

    private bool _groundDetected = false;
    private bool _wallDetected = false;

    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Health _health;
    private ReceiveKnockback _knockback;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _knockback = GetComponent<ReceiveKnockback>();
    }

    private void OnEnable()
    {
        _health.Died.AddListener(OnDied);

        _knockback.KnockbackStarted += OnKnockbackStarted;
        _knockback.KnockbackEnded += OnKnockbackEnded;
    }

    private void OnDisable()
    {
        _health.Died.RemoveListener(OnDied);

        _knockback.KnockbackStarted -= OnKnockbackStarted;
        _knockback.KnockbackEnded -= OnKnockbackEnded;
    }

    void Update()
    {
        switch (_currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    // walking state
    private void EnterMovingState()
    {
        //TODO walk animation
        
    }

    private void UpdateMovingState()
    {
        _groundDetected = Physics2D.Raycast
            (_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround);
        _wallDetected = Physics2D.Raycast
            (_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
        // if no ground or wall detected
        if(!_groundDetected || _wallDetected)
        {
            // flip
            Flip();
        }
        // otherwise, continue moving
        else
        {
            // move enemy
            _movement.Set(_movementSpeed * _facingDirection, _rb.velocity.y);
            _rb.velocity = _movement;
        }
    }

    private void ExitMovingState()
    {

    }

    // knockback state
    private void EnterKnockbackState()
    {
        //TODO flinch animation
        _animator.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {

    }

    private void ExitKnockbackState()
    {
        _animator.SetBool("Knockback", false);
    }

    // dead state
    private void EnterDeadState()
    {
        //TODO spawn dead particles
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    // private methods
    private void SwitchState(State newState)
    {
        switch (_currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        _currentState = newState;

        switch (_currentState)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
    }

    private void Flip()
    {
        _facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDied()
    {

    }

    private void OnKnockbackStarted()
    {
        SwitchState(State.Knockback);
    }

    private void OnKnockbackEnded()
    {
        SwitchState(State.Moving);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2
            (_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2
            (_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
    }
}
