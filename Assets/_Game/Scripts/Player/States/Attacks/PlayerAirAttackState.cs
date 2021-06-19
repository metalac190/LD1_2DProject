using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private PlayerData _data;
    private MovementKM _movement;
    private GameplayInput _input;
    private WeaponSystem _weaponSystem;
    private DashSystem _dashSystem;
    private WeaponData _weaponData;
    private GroundDetector _groundDetector;

    bool _attackInputBuffer = false;
    bool _hitDamageable = false;
    private bool _isInitialAttack = true;

    public PlayerAirAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _movement = player.Movement;
        _input = player.Input;
        _weaponSystem = player.WeaponSystem;
        _dashSystem = player.DashSystem;

        _weaponData = player.WeaponSystem.EquippedWeapon;
        _groundDetector = player.CollisionDetector.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Air Attack");

        _weaponSystem.AttackCompleted += OnAttackCompleted;
        _weaponSystem.AttackDeactivated += OnAttackDeactivated;
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        _groundDetector.FoundGround += OnFoundGround;
        _weaponSystem.HitDamageable += OnHitDamageable;

        _hitDamageable = false;
        // start at normal fall speed
        _movement.SetGravityScale(1);

        _isInitialAttack = true;
        Attack();
        _isInitialAttack = false;
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;
        _weaponSystem.AttackDeactivated -= OnAttackDeactivated;
        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _groundDetector.FoundGround -= OnFoundGround;
        _weaponSystem.HitDamageable -= OnHitDamageable;

        // resume
        _movement.SetGravityScale(1);

        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();

        // if attack is active propel forward, if it's in wepaon data
        if (_weaponSystem.MeleeAttackState == MeleeAttackState.DuringAttack
            && _hitDamageable)
        {
            // if we're holding forward, apply additional movement based on weapon settings
            if(_input.XInputRaw == _movement.FacingDirection)
            {
                // forward but disable falling
                _movement.MoveX((_weaponSystem.CurrentMeleeAttack.ForwardAmount 
                    * _movement.FacingDirection) + (_movement.FacingDirection * _data.MoveSpeed) 
                    * _weaponSystem.CurrentMeleeAttack.MovementReductionRatio);
            }
            else
            {
                _movement.MoveX((_weaponSystem.CurrentMeleeAttack.ForwardAmount 
                    * _movement.FacingDirection));
            }

        }
        // otherwise just move according to input
        else
        {
            _movement.MoveX(_input.XInputRaw * _data.MoveSpeed);
        }
    }

    public override void Update()
    {
        base.Update();

        // if we've changed directions
        if (_input.XInputRaw == (_movement.FacingDirection * -1)
            && _weaponSystem.MeleeAttackState == MeleeAttackState.BeforeAttack)
        {
            // and we're still ramping up, switch directions
            _movement.Flip();
        }

        // if we've completed the attack and have attack input buffer, attack again
        else if (_attackInputBuffer && _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack
            && _weaponSystem.AttackCount < _weaponData.MaxComboCount)
        {
            if(_weaponSystem.AttackCount == _weaponData.MaxComboCount - 1)
            {
                FinisherAttack();
            }
            else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        _attackInputBuffer = false;
        _hitDamageable = false;

        _weaponSystem.StartAttack(_weaponSystem.EquippedWeapon.AirAttack, 
            _weaponSystem.EquippedWeapon.HitSFX, _isInitialAttack);
    }

    private void FinisherAttack()
    {
        _attackInputBuffer = false;
        _hitDamageable = false;

        _weaponSystem.StartAttack(_weaponSystem.EquippedWeapon.AirFinisher,
            _weaponSystem.EquippedWeapon.FinisherSFX, _isInitialAttack);
        _isInitialAttack = true;
    }

    private void OnHitDamageable(IDamageable damageable)
    {
        Debug.Log("Hit Damageable");
        _hitDamageable = true;
        // if we've hit something, reset air dash
        _dashSystem.ReadyDash();
        _movement.SetGravityScale(0);
        _movement.SetVelocityZero();
    }

    private void OnAttackCompleted()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }

    private void OnAttackDeactivated()
    {
        _movement.SetGravityScale(1);
    }

    private void OnAttackPressed()
    {
        if (_weaponSystem.MeleeAttackState == MeleeAttackState.DuringAttack
            || _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack)
        {
            _attackInputBuffer = true;
        }
    }

    private void OnJumpPressed()
    {
        if (_data.AllowJump && _player.AirJumpsRemaining > 0)
        {
            _stateMachine.ChangeState(_stateMachine.AirJumpState);
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

    private void OnFoundGround()
    {
        _stateMachine.ChangeState(_stateMachine.LandState);
    }
}
