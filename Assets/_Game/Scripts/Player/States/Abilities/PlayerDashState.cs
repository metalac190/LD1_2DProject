using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private PlayerData _data;
    private Rigidbody2D _rb;
    private DashSystem _dashSystem;
    private GameplayInput _input;
    private GroundDetector _groundDetector;

    private bool _isDashing = false;
    private float _holdTimer = 0;
    private float _dashTimer = 0;
    private Vector2 _dashDirection;

    private float _initialDrag;
    private float _initialFixedDeltaTime;

    private PlayerAfterImagePool _afterImagePool;
    private PlayerAfterImage _lastAfterImage;

    public PlayerDashState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _rb = player.RB;
        _dashSystem = player.DashSystem;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
        _afterImagePool = _dashSystem.AfterImagePool;

        _initialDrag = player.RB.drag;
        _initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Dash");
        base.Enter();

        //
        _input.DashReleased += OnDashInputReleased;

        _isDashing = false;
        _holdTimer = 0;
        _dashTimer = 0;

        StartHold();
    }

    public override void Exit()
    {
        base.Exit();

        _input.DashReleased -= OnDashInputReleased;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // using the dash in update so that we don't trigger a state change while we're still entering
        _groundDetector.DetectGround();

        if (_isDashing)
        {
            _player.SetVelocity(_dashDirection.x * _data.DashVelocity, _dashDirection.y * _data.DashVelocity);
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
            UseDash();
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
        Time.timeScale = _data.HoldTimeScale;
        Time.fixedDeltaTime = _initialFixedDeltaTime * Time.timeScale;
    }

    private void OnDashInputReleased()
    {
        UseDash();
    }

    private void UseDash()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = _initialFixedDeltaTime;

        _isDashing = true;
        _dashDirection = _input.Movement;
        // ensure we still dash, even without input
        if(_dashDirection == Vector2.zero)
        {
            _dashDirection = new Vector2(_player.FacingDirection, 0);
        }

        _rb.drag = _data.DashDrag;

        _lastAfterImage = _afterImagePool.PlaceAfterImage(_player);
    }

    private void CompleteDash()
    {
        _dashSystem.StartCooldown(_data.DashCooldown);
        _rb.drag = _initialDrag;
        _player.SetVelocityY(_data.DashEndYMultiplier * _rb.velocity.y);

        Debug.Log("Dash Completed");
        if (_groundDetector.IsGrounded && _input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        else if(_groundDetector.IsGrounded && _input.XRaw == 0)
        {
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
