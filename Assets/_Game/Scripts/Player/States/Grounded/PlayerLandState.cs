using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;
    Player _player;

    GameplayInput _input;
    PlayerData _data;
    PlayerSFXData _sfx;
    DashSystem _dashSystem;

    public PlayerLandState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _sfx = player.SFX;
        _dashSystem = player.DashSystem;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Land");
        base.Enter();

        _player.ResetJumps();
        _dashSystem.ReadyDash();

        _sfx.LandSFX.PlayOneShot(_player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        // for now land immediately, but consider adding animations and whatnot later
        if(_input.XInputRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
            return;
        }

        else if(StateDuration >= _data.LandDuration && _input.XInputRaw == 0)
        {
            //TODO: play animations, wait, then transition to idle
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }
}
