using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallAttackState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    private Movement _movement;
    private WeaponSystem _weaponSystem;

    Vector2 _startPos;

    public PlayerWallAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _weaponSystem = player.WeaponSystem;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Wall Attack");
        base.Enter();

        _weaponSystem.AttackCompleted += OnAttackCompleted;

        _startPos = _movement.Position;
        _movement.HoldPosition(_startPos);

        _movement.Flip();
        _weaponSystem.StartAttack(_weaponSystem.EquippedWeapon.WallAttack, _weaponSystem.EquippedWeapon.HitSFX);
    }

    public override void Exit()
    {
        base.Exit();

        _weaponSystem.AttackCompleted -= OnAttackCompleted;

        _movement.Flip();
        _movement.HoldPosition(_startPos);
        _weaponSystem.StopAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _movement.HoldPosition(_startPos);
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnAttackCompleted()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
