using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private GameplayInput _input;
    private PlayerData _data;
    private GroundDetector _groundDetector;
    private CeilingDetector _ceilingDetector;
    private DashSystem _dashSystem;

    public PlayerGroundedSuperState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.CollisionDetector.GroundDetector;
        _ceilingDetector = player.CollisionDetector.CeilingDetector;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();

        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        _groundDetector.LeftGround += OnLeftGround;
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _groundDetector.LeftGround -= OnLeftGround;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.DetectGround();

    }

    public override void Update()
    {
        base.Update();

    }

    private void OnAttackPressed()
    {
        _ceilingDetector.DetectCeiling();
        if (!_ceilingDetector.IsTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.GroundAttackState);
        }
    }

    private void OnJumpPressed()
    {
        _ceilingDetector.DetectCeiling();
        if (_data.AllowJump && !_ceilingDetector.IsTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    private void OnDashPressed()
    {
        _ceilingDetector.DetectCeiling();
        if (_dashSystem.IsReady && !_ceilingDetector.IsTouchingCeiling)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    private void OnLeftGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }


}
