using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private PlayerData _data;
    private Movement _movement;
    private GameplayInput _input;
    private WeaponSystem _weaponSystem;
    private DashSystem _dashSystem;
    private WeaponData _weaponData;
    private GroundDetector _groundDetector;

    bool _attackInputBuffer = false;

    public PlayerAirAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _movement = player.Actor.Movement;
        _input = player.Input;
        _weaponSystem = player.WeaponSystem;
        _dashSystem = player.DashSystem;

        _weaponData = player.WeaponSystem.EquippedWeapon;
        _groundDetector = player.Actor.CollisionDetector.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Air Attack");

        _weaponSystem.AttackCompleted += OnAttackCompleted;
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        _groundDetector.FoundGround += OnFoundGround;

        _weaponSystem.StartAirAttack();

        _attackInputBuffer = false;
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;
        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _groundDetector.FoundGround -= OnFoundGround;

        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();

        // if attack is active propel forward, if it's in wepaon data
        if (_weaponSystem.MeleeAttackState == MeleeAttackState.DuringAttack
            && _input.XInputRaw != 0)
        {
            _movement.SetVelocityX((_weaponData.AirForwardAmount * _movement.FacingDirection) 
                + (_input.XInput * _data.MoveSpeed));
        }
        // otherwise just move according to input
        else
        {
            _movement.SetVelocityX(_input.XInputRaw * _data.MoveSpeed);
        }
    }

    public override void Update()
    {
        base.Update();

        // if we've changed directions
        if (_input.XInputRaw == (_movement.FacingDirection * -1))
        {
            // and we're still ramping up, switch directions
            if (_weaponSystem.MeleeAttackState == MeleeAttackState.BeforeAttack)
            {
                _movement.Flip();
            }
            // or if our attack is active, cancel it
            else
            {
                _weaponSystem.StopAttack();
                _stateMachine.ChangeState(_stateMachine.FallingState);
                return;
            }
        }

        // if we've completed the attack and have attack input buffer, attack again
        if (_attackInputBuffer && _weaponSystem.MeleeAttackState == MeleeAttackState.AfterAttack)
        {
            Debug.Log("After attack buffer, start new attack!");
            // start new attack and reset input
            _weaponSystem.StartAirAttack();
            _attackInputBuffer = false;
        }
    }

    private void OnAttackCompleted()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
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
        if (_data.AllowDash && _dashSystem.CanDash)
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
