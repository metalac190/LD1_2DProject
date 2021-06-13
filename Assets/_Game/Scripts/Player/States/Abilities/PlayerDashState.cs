using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private Movement _movement;
    private PlayerData _data;
    private Rigidbody2D _rb;
    private DashSystem _dashSystem;
    private GameplayInput _input;
    private GroundDetector _groundDetector;
    private PlayerSFXData _sfx;

    private bool _isDashing = false;
    private float _holdTimer = 0;
    private float _dashTimer = 0;
    private Vector2 _dashDirection;

    private float _initialDrag;
    private float _initialGravityScale;

    private PlayerAfterImagePool _afterImagePool;
    private PlayerAfterImage _lastAfterImage;

    public PlayerDashState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _rb = player.Actor.Movement.RB;
        _dashSystem = player.DashSystem;
        _input = player.Input;
        _groundDetector = player.Actor.CollisionDetector.GroundDetector;
        _sfx = player.SFX;

        _afterImagePool = _dashSystem.AfterImagePool;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Dash");
        base.Enter();

        //
        _input.DashReleased += OnDashInputReleased;
        _input.AttackPressed += OnAttackPressed;

        _isDashing = false;
        _holdTimer = 0;
        _dashTimer = 0;
        _initialDrag = _rb.drag;
        _initialGravityScale = _rb.gravityScale;

        _dashSystem.UseDash();
        StartHold();
    }

    public override void Exit()
    {
        base.Exit();

        _input.DashReleased -= OnDashInputReleased;
        _input.AttackPressed -= OnAttackPressed;

        // ensure changed gravity values are returned
        _rb.gravityScale = _initialGravityScale;
        _rb.drag = _initialDrag;
        _movement.SetVelocityY(_data.DashEndYMultiplier * _rb.velocity.y);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // using the dash in update so that we don't trigger a state change while we're still entering
        _groundDetector.DetectGround();
        // if we're prepping the dash, minimize movement with our hold dash dampener - y is still handled by gravity
        if (!_isDashing)
        {
            _movement.SetVelocityX(_input.XInputRaw * _data.MoveSpeed * _data.DashHoldMovementDampener);
        }
        // otherwise use dash values
        else if (_isDashing)
        {
            _movement.SetVelocity(_dashDirection.x * _data.DashVelocity, _dashDirection.y * _data.DashVelocity);
            CheckAfterImageSpawn();

        }
    }

    private void CheckAfterImageSpawn()
    {
        if (_lastAfterImage != null)
        {
            float imageDistance = Vector2.Distance(_player.transform.position, _lastAfterImage.transform.position);
            if (imageDistance >= _data.DistanceBetweenAfterImages)
            {
                _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);
            }
        }
    }

    public override void Update()
    {
        base.Update();

        IncrementTimers();

        if(!_isDashing && _holdTimer >= _data.MaxHoldTime)
        {
            DashInDirection();
        }
        else if(_isDashing && _dashTimer >= _data.DashDuration)
        {
            CompleteDash();
        }
    }

    private void IncrementTimers()
    {
        if (!_isDashing)
        {
            _holdTimer += Time.unscaledDeltaTime;
        }
        else if (_isDashing)
        {
            _dashTimer += Time.deltaTime;
        }
    }

    private void StartHold()
    {
        _rb.gravityScale = _rb.gravityScale * _data.DashHoldMovementDampener;
        // kill current velocity
        _movement.SetVelocityZero();

        _sfx.DashHoldSFX?.PlayOneShot(_player.transform.position);
    }

    private void OnDashInputReleased()
    {
        DashInDirection();
    }

    private void OnAttackPressed()
    {
        if (!_data.AllowAttack) { return; }

        if (!_groundDetector.IsGrounded)
        {
            _stateMachine.ChangeState(_stateMachine.AirAttackState);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.GroundAttackState);
        }
    }

    private void DashInDirection()
    {
        _rb.gravityScale = _initialGravityScale;

        _isDashing = true;
        _dashDirection = _input.MoveInput;
        // ensure we still dash, even without input
        if(_dashDirection == Vector2.zero)
        {
            _dashDirection = new Vector2(_movement.FacingDirection, 0);
        }

        _rb.drag = _data.DashDrag;

        _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);

        _sfx.DashReleaseSFX?.PlayOneShot(_player.transform.position);
    }

    private void CompleteDash()
    {
        _rb.drag = _initialDrag;
        _movement.SetVelocityY(_data.DashEndYMultiplier * _rb.velocity.y);

        // completed dash. Decide where to transition from here
        if (_groundDetector.IsGrounded && _input.XInputRaw != 0)
        {
            _dashSystem.ReadyDash();
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        else if(_groundDetector.IsGrounded && _input.XInputRaw == 0)
        {
            _dashSystem.ReadyDash();
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }
    }

}
