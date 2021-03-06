using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedSuperState
{
    PlayerFSM _stateMachine;
    Player _player;
    PlayerAnimator _animator;

    GameplayInput _input;
    PlayerMoveData _data;
    PlayerSFXData _sfx;
    DashSystem _dashSystem;
    ParticleSystem _jumpDust;

    public PlayerLandState(PlayerFSM stateMachine, Player player) : base(stateMachine, player)
    {
        _stateMachine = stateMachine;
        _player = player;
        _animator = player.PlayerAnimator;

        _input = player.Input;
        _data = player.Data;
        _sfx = player.SFX;
        _dashSystem = player.DashSystem;
        _jumpDust = player.Visuals.JumpDust;
    }

    public override void Enter()
    {
        Debug.Log("STATE: Land");
        base.Enter();

        _animator.PlayAnimation(PlayerAnimator.LandName);
        _player.ResetJumps();

        _sfx.LandSFX.PlayOneShot(_player.transform.position);
        _jumpDust?.Play();
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
