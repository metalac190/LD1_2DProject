using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private MovementKM _movement;
    private PlayerData _data;
    private DashSystem _dashSystem;
    private GameplayInput _input;
    private GroundDetector _groundDetector;
    private PlayerSFXData _sfx;

    private bool _isDashing = false;
    private float _holdTimer = 0;
    private float _dashTimer = 0;
    private Vector2 _dashDirection;

    private PlayerAfterImagePool _afterImagePool;
    private PlayerAfterImage _lastAfterImage;

    public PlayerDashState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Movement;
        _data = player.Data;
        _dashSystem = player.DashSystem;
        _input = player.Input;
        _groundDetector = player.CollisionDetector.GroundDetector;
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

        _dashSystem.UseDash();

        StartHold();
    }

    public override void Exit()
    {
        base.Exit();

        _input.DashReleased -= OnDashInputReleased;
        _input.AttackPressed -= OnAttackPressed;

        // ensure changed movement and gravity values are returned
        //_movement.SetVelocity(_data.MoveSpeed * _input.XInputRaw * _data.DashEndScale, 0);
        _movement.SetVelocityZero();
        _movement.SetGravityScale(1);
        Debug.Log("Exit: " + _movement.Velocity);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // using the dash in update so that we don't trigger a state change while we're still entering
        _groundDetector.DetectGround();
        // if we're prepping the dash, minimize movement with our hold dash dampener - y is still handled by gravity
        if (!_isDashing)
        {
            _movement.MoveX(_input.XInputRaw * _data.MoveSpeed * _data.DashHoldMovementScale);
        }
        // otherwise use dash values
        else if (_isDashing)
        {
            _movement.Move(_dashDirection * _data.DashVelocity);
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
        _movement.SetGravityScale(_data.DashHoldMovementScale);
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
        _isDashing = true;
        _dashDirection = _input.MoveInput;
        // ensure we still dash, even without input
        //TODO: consider... if there's no input, should we instead do a spot dodge or parry?
        if(_dashDirection == Vector2.zero)
        {
            _dashDirection = new Vector2(_movement.FacingDirection, 0);
        }
        _movement.SetGravityScale(_data.DashingGravityScale);

        _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);

        _sfx.DashReleaseSFX?.PlayOneShot(_player.transform.position);
    }

    private void CompleteDash()
    {
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
