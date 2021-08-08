using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedSuperState
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private MovementKM _movement;
    private GameplayInput _input;
    private PlayerData _data;
    private PlayerAnimator _playerAnimator;
    private OverlapDetector _ceilingDetector;

    public PlayerCrouchMoveState(PlayerFSM stateMachine, Player player) 
        : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Movement;
        _input = player.Input;
        _data = player.Data;
        _playerAnimator = player.PlayerAnimator;
        _ceilingDetector = player.EnvironmentDetector.CeilingDetector;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Move Crouch");

        _player.SetColliderHeight(_data.CrouchColliderHeight);
        _playerAnimator.ShowCrouchVisual(true);

        _ceilingDetector.Detect();
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

        _ceilingDetector.Detect();

        _movement.MoveX(_data.CrouchMoveVelocity * _input.XInputRaw, true);
        // if we're not holding down and holding either direction
        // AND we're not touching the ceiling
        if (_input.YInputRaw != -1 && _input.XInputRaw != 0
            && _ceilingDetector.IsDetected == false)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        // if we're not holding down or side directions, and not touching the ceiling
        else if (_input.YInputRaw >= 0 && _input.XInputRaw == 0
            && _ceilingDetector.IsDetected == false) 
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }

    }

    public override void Update()
    {
        base.Update();

        _movement.CheckIfShouldFlip(_input.XInputRaw);
        // if we're holding downwards
        if (_input.YInputRaw <= 0 && _input.XInputRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.CrouchState);
            return;
        }
        // if we're moving sideways, and not touching the ceiling



    }
}
