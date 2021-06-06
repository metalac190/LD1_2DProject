using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : State
{
    private PlayerFSM _stateMachine;

    private Movement _movement;
    private PlayerData _data;
    private GameplayInput _input;
    private WeaponSystem _weaponSystem;
    private DashSystem _dashSystem;
    private WeaponData _weaponData;
    private GroundDetector _groundDetector;

    bool _attackInputBuffer = false;

    public PlayerGroundAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _input = player.Input;
        _weaponSystem = player.WeaponSystem;
        _dashSystem = player.DashSystem;
        _weaponData = player.WeaponSystem.EquippedWeapon;
        _groundDetector = player.Actor.CollisionDetector.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Attack");

        _weaponSystem.AttackCompleted += OnAttackCompleted;
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        _groundDetector.LeftGround += OnLeftGround;

        _attackInputBuffer = false;
        _weaponSystem.StartGroundAttack();
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;
        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _groundDetector.LeftGround -= OnLeftGround;

        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _groundDetector.DetectGround();

        // if attack is active, propel forward based on weapon data forward amount
        if (_weaponSystem.MeleeAttackState == MeleeAttackState.DuringAttack)
        {
            // if we're holding forward, add additional movement
            if(_input.XInputRaw == _movement.FacingDirection)
            {
                _movement.SetVelocityX((_weaponData.GroundForwardAmount * _movement.FacingDirection)
                    + (_movement.FacingDirection * _data.MoveSpeed * _weaponData.MovementReductionRatio));
            }
            // otherwise just use forward amount
            else
            {
                _movement.SetVelocityX((_weaponData.GroundForwardAmount * _movement.FacingDirection));
            }
        }
        // otherwise move forward with normal movement speed cut by weapon reduction speed
        else if(_input.XInputRaw == _movement.FacingDirection)
        {
            _movement.SetVelocityX(_input.XInputRaw * _data.MoveSpeed 
                * _weaponData.MovementReductionRatio);
        }
        // otherwise no xinput, so don't move in x
        else
        {
            _movement.SetVelocityX(0);
        }
    }

    public override void Update()
    {
        base.Update();

        // if we've changed directions
        if(_input.XInputRaw == (_movement.FacingDirection * -1)
            && _weaponSystem.MeleeAttackState == MeleeAttackState.BeforeAttack)
        {
            // and we're still ramping up, switch directions
            _movement.Flip();
        }

        // if we've received new attack input during the post attack period
        if(_attackInputBuffer && _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack)
        {
            // start new attack and reset input
            _weaponSystem.StartGroundAttack();
            _attackInputBuffer = false;
        }
    }

    private void OnAttackCompleted()
    {
        if(_input.XInput != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }

    private void OnAttackPressed()
    {
        // allow new attack input during/after attack, briefly
        if(_weaponSystem.MeleeAttackState == MeleeAttackState.DuringAttack
            || _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack)
        {
            _attackInputBuffer = true;
        }
        
    }

    private void OnJumpPressed()
    {
        if (_data.AllowJump)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
            return;
        }
    }

    private void OnDashPressed()
    {
        if (_data.AllowDash && _dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
            return;
        }
    }

    private void OnLeftGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
