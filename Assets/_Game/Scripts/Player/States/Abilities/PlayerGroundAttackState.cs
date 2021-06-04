using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private PlayerData _data;
    private GameplayInput _input;
    private WeaponSystem _weaponSystem;
    private DashSystem _dashSystem;
    private WeaponData _weaponData;
    private GroundDetector _groundDetector;

    private bool _savedJumpInput = false;
    private bool _savedDashInput = false;

    public PlayerGroundAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _weaponSystem = player.WeaponSystem;
        _dashSystem = player.DashSystem;
        _weaponData = player.WeaponSystem.EquippedWeapon;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Attack");

        _weaponSystem.AttackCompleted += OnAttackCompleted;
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _groundDetector.LeftGround += OnLeftGround;

        _savedJumpInput = false;
        _savedDashInput = false;

        _weaponSystem.StartAttack();
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;
        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _groundDetector.LeftGround -= OnLeftGround;

        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _groundDetector.DetectGround();

        // if attack is active propel forward, if it's in wepaon data
        if (_weaponSystem.IsAttackActive)
        {
            _player.SetVelocityX(_weaponData.GroundForwardAmount * _player.FacingDirection);
        }
        else
        {
            _player.SetVelocityX(0);
        }
        // otherwise just move forward according to player controls
    }

    public override void Update()
    {
        base.Update();

        // if we've changed directions
        if(_input.XInputRaw == (_player.FacingDirection * -1))
        {
            // and we're still ramping up, switch directions
            if (_weaponSystem.IsPreAttack)
            {
                _player.Flip();
            }
            // or if our attack is active, cancel it
            else 
            {
                _weaponSystem.StopAttack();
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
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
        if (_dashSystem.CanDash)
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
