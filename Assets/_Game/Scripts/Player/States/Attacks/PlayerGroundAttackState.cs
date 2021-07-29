using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : State
{
    private PlayerFSM _stateMachine;
    private PlayerAnimator _animator;

    private MovementKM _movement;
    private PlayerData _data;
    private GameplayInput _input;
    private WeaponSystem _weaponSystem;
    private DashSystem _dashSystem;
    private WeaponData _weaponData;
    private GroundDetector _groundDetector;

    bool _attackInputBuffer = false;
    private bool _isInitialAttack = true;

    public PlayerGroundAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _animator = player.PlayerAnimator;

        _movement = player.Movement;
        _data = player.Data;
        _input = player.Input;
        _weaponSystem = player.WeaponSystem;
        _dashSystem = player.DashSystem;
        _weaponData = player.WeaponSystem.EquippedWeapon;
        _groundDetector = player.CollisionDetector.GroundDetector;
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

        // it's possible we could cancel dash with a ground attack. make sure we refresh it

        _isInitialAttack = true;
        Attack();
        _isInitialAttack = false;
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
            // if we're holding forward, add additional movement based on our attacks move reduction
            if(_input.XInputRaw == _movement.FacingDirection)
            {
                float moveAmount = (_weaponSystem.CurrentMeleeAttack.PlayerForwardAmount
                    * _movement.FacingDirection) + (_movement.FacingDirection * _data.MoveSpeed
                    * _weaponSystem.CurrentMeleeAttack.MovementReductionRatio);
                //TODO: Consider if this can be a force with momentum, once we have that capability
                _movement.MoveX(moveAmount, false);
            }
            // otherwise just use forward amount
            else
            {
                float moveAmount = (_weaponSystem.CurrentMeleeAttack.PlayerForwardAmount
                    * _movement.FacingDirection);
                
                _movement.MoveX(moveAmount, false);
            }
        }
        // otherwise no xinput, so don't move in x
        else
        {
            _movement.MoveX(0, false);
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
        if(_attackInputBuffer && _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack
            && _weaponSystem.AttackCount < _weaponData.MaxComboCount)
        {
            // if next attack will be our last attack, do the finisher
            if (_weaponSystem.AttackCount == _weaponData.MaxComboCount - 1)
            {
                FinisherAttack();
            }
            // otherwise do another attack
            else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        _attackInputBuffer = false;
        _weaponSystem.StandardAttack(_weaponSystem.EquippedWeapon.GroundAttack, 
            _weaponSystem.EquippedWeapon.HitSFX, _isInitialAttack);
    }

    private void FinisherAttack()
    {
        _attackInputBuffer = false;
        _weaponSystem.StandardAttack(_weaponSystem.EquippedWeapon.GroundFinisher, 
            _weaponSystem.EquippedWeapon.FinisherSFX, _isInitialAttack);

        _isInitialAttack = true;
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
        if (_data.AllowDash && _dashSystem.IsReady)
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
