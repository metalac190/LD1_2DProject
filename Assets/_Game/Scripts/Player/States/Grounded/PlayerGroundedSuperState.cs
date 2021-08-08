using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private GameplayInput _input;
    private PlayerData _data;
    private OverlapDetector _groundDetector;
    private OverlapDetector _ceilingDetector;
    private DashSystem _dashSystem;

    public PlayerGroundedSuperState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.EnvironmentDetector.GroundDetector;
        _ceilingDetector = player.EnvironmentDetector.CeilingDetector;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();

        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;
        _input.AttackPressed += OnAttackPressed;
        _groundDetector.LostCollider += OnLostGround;
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
        _input.AttackPressed -= OnAttackPressed;
        _groundDetector.LostCollider -= OnLostGround;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _groundDetector.Detect();

    }

    public override void Update()
    {
        base.Update();

    }

    private void OnAttackPressed()
    {
        _ceilingDetector.Detect();
        if (_ceilingDetector.IsDetected) { return; }

        _stateMachine.ChangeState(_stateMachine.GroundAttackState);
    }

    private void OnJumpPressed()
    {
        _ceilingDetector.Detect();

        if (_data.AllowJump && _ceilingDetector.IsDetected == false)
        {
            _stateMachine.ChangeState(_stateMachine.JumpState);
        }
    }

    private void OnDashPressed()
    {
        _ceilingDetector.Detect();
        if (_dashSystem.IsReady && _ceilingDetector.IsDetected == false)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }

    private void OnLostGround()
    {
        _stateMachine.ChangeState(_stateMachine.FallingState);
    }


}
