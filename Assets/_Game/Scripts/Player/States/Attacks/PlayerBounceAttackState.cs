using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceAttackState : State
{
    PlayerFSM _stateMachine;

    KinematicObject _movement;
    PlayerData _data;
    WeaponSystem _weaponSystem;
    GameplayInput _input;
    // this prevents us from getting mutliple calls in a single bounce attack
    bool _usedBounce = false;

    public PlayerBounceAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;

        _movement = player.Movement;
        _data = player.Data;
        _weaponSystem = player.WeaponSystem;
        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Bounce Attack");
        _weaponSystem.AttackCompleted += OnAttackCompleted;
        _weaponSystem.HitOther += OnHitOther;
        _input.AttackPressed += OnAttackPressed;

        _usedBounce = false;

        Attack();
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;
        _weaponSystem.HitOther -= OnHitOther;
        _input.AttackPressed -= OnAttackPressed;
        // make sure that if we leave early, we cancel the attack in the weapon system
        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _movement.MoveX(_input.XInputRaw * _data.MoveSpeed, true);
    }

    public override void Update()
    {
        base.Update();

    }

    private void Attack()
    {
        Debug.Log("Bounce attack!");
        _weaponSystem.BounceAttack(_weaponSystem.EquippedWeapon.AirAttack,
            _weaponSystem.EquippedWeapon.HitSFX);
    }

    private void OnAttackPressed()
    {
        _movement.MoveY(_movement.Velocity.y * _data.AirAttackVelocityYDampen);
        _stateMachine.ChangeState(_stateMachine.AirAttackState);
    }

    private void OnHitOther()
    {
        // launch player upward!
        if(_usedBounce == false)
        {
            Bounce();
        }
    }

    private void Bounce()
    {
        _usedBounce = true;
        _movement.MoveY(_data.BounceAttackVertical);
    }

    private void OnAttackCompleted()
    {
        if (_movement.IsGrounded)
            _stateMachine.ChangeState(_stateMachine.LandState);
        else
            _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
