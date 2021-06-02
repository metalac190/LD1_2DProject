using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    PlayerData _data;
    DashSystem _dashSystem;

    private float _landDuration = .2f ;

    public PlayerLandState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Land");
        
        _input.JumpPressed += OnJumpPressed;
        _input.DashPressed += OnDashPressed;

        _player.ResetJumps();
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.DashPressed -= OnDashPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        // for now land immediately, but consider adding animations and whatnot later
        if(_input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }

        if(StateDuration >= _data.LandDuration && _input.XRaw == 0)
        {
            //TODO: play animations, wait, then transition to idle
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void OnJumpPressed()
    {
        // jump
        _stateMachine.ChangeState(_stateMachine.JumpState);
    }

    private void OnDashPressed()
    {
        if (_dashSystem.CanDash)
        {
            _stateMachine.ChangeState(_stateMachine.DashState);
        }
    }
}
