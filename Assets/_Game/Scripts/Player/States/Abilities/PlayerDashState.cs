using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    private DashSystem _dashSystem;
    private PlayerDashAbility _dashAbility;
    private GameplayInput _input;
    private GroundDetector _groundDetector;

    private bool _dashUsed = false;

    public PlayerDashState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _dashSystem = player.DashSystem;
        _input = player.Input;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Dash");
        base.Enter();

        _dashAbility = _dashSystem.EquippedDash;

        _dashAbility.Completed += OnCompletedDash;
        _input.DashReleased += OnDashInputReleased;

        _dashAbility.OnInputPress(_player);
    }

    public override void Exit()
    {
        base.Exit();

        _dashAbility.Completed -= OnCompletedDash;
        _input.DashReleased -= OnDashInputReleased;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // using the dash in update so that we don't trigger a state change while we're still entering
        _groundDetector.DetectGround();

        _dashAbility.OnFixedUpdate(_player);
        _dashUsed = true;
    }

    public override void Update()
    {
        base.Update();

        _dashAbility.OnUpdate(_player);
    }

    private void OnCompletedDash()
    {
        _dashSystem.StartCooldown();


        Debug.Log("Dash Completed");
        if (_groundDetector.IsGrounded && _input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }
        else if(_groundDetector.IsGrounded && _input.XRaw == 0)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }
    }

    private void OnDashInputReleased()
    {
        Debug.Log("Dash Released");
        _dashAbility.OnInputRelease(_player);
    }
}
