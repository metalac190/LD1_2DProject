using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedSuperState
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private GameplayInput _input;
    private PlayerData _data;
    private PlayerAnimator _playerAnimator;
    private CeilingDetector _ceilingDetector;

    public PlayerCrouchMoveState(PlayerFSM stateMachine, Player player) 
        : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _playerAnimator = player.PlayerAnimator;
        _ceilingDetector = player.CeilingDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Move Crouch");

        _player.SetColliderHeight(_data.CrouchColliderHeight);
        _playerAnimator.ShowCrouchVisual(true);

        _ceilingDetector.DetectCeiling();
    }

    public override void Exit()
    {
        base.Exit();

        _player.SetColliderHeight(_data.StandColliderHeight);
        _playerAnimator.ShowCrouchVisual(false);
    }

    public override void FixedUpdate()
    {

        base.FixedUpdate();

        _player.SetVelocityX(_data.CrouchMoveVelocity * _player.FacingDirection);
        _ceilingDetector.DetectCeiling();
        // if we're not holding down and holding either direction
        // AND we're not touching the ceiling
        if (_input.YRaw != -1 && _input.XRaw != 0
            && !_ceilingDetector.IsTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        // if we're not holding down or side directions, and not touching the ceiling
        else if (_input.YRaw >= 0 && _input.XRaw == 0
            && !_ceilingDetector.IsTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }

    }

    public override void Update()
    {
        base.Update();

        // if we're holding downwards
        if (_input.YRaw <= 0 && _input.XRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        // if we're moving sideways, and not touching the ceiling



    }
}
