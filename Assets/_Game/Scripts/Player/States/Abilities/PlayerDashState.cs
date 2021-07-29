using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;
    private PlayerAnimator _animator;

    private MovementKM _movement;
    private PlayerData _data;
    private DashSystem _dashSystem;
    private GameplayInput _input;
    private GroundDetector _groundDetector;
    private PlayerSFXData _sfx;

    private Vector2 _dashDirection;

    public PlayerDashState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _data = player.Data;
        _dashSystem = player.DashSystem;
        _input = player.Input;
        _groundDetector = player.CollisionDetector.GroundDetector;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Dash");
        base.Enter();

        _animator.PlayAnimation(PlayerAnimator.DashingName);

        _input.AttackPressed += OnAttackPressed;
        _dashSystem.DashCompleted += OnDashCompleted;

        DetermineDashDirection();

        _dashSystem.UseDash(_data.DashDuration, _data.DashCooldown);

        _movement.SetVelocityZero();
        _movement.SetGravityScale(_data.DashingGravityScale);
        _sfx.DashReleaseSFX?.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();

        _input.AttackPressed -= OnAttackPressed;
        _dashSystem.DashCompleted -= OnDashCompleted;

        _dashSystem.StopDash(_data.DashCooldown);
        // ensure changed movement and gravity values are returned
        _movement.SetVelocityZero();
        _movement.SetGravityScale(1);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // using the dash in update so that we don't trigger a state change while we're still entering
        _groundDetector.DetectGround();

        // otherwise use dash values
        _movement.Move(_dashDirection * _data.DashVelocity, false);
    }

    public override void Update()
    {
        base.Update();

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

    private void DetermineDashDirection()
    {
        //TODO: consider... if there's no input, should we instead do a spot dodge or parry?
        // ensure we still dash, even without input
        if (!_data.AllowDirectionInput || _input.MoveInput == Vector2.zero)
        {
            _dashDirection = new Vector2(_movement.FacingDirection, 0);
        }
        else
        {
            _dashDirection = _input.MoveInput;
        }
    }

    private void OnDashCompleted()
    {
        // completed dash. Decide where to transition from here
        if (_groundDetector.IsGrounded && _input.XInputRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        else if(_groundDetector.IsGrounded && _input.XInputRaw == 0)
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
