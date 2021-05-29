using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    InputManager _input;
    PlayerData _data;

    private float _landDuration = .2f ;

    public PlayerLandState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Land");
        
        _input.SpacebarPressed += OnSpacebarPressed;

        _player.ResetJumps();
    }

    public override void Exit()
    {
        base.Exit();

        _input.SpacebarPressed -= OnSpacebarPressed;
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

    private void OnSpacebarPressed()
    {
        // jump
        _stateMachine.ChangeState(_stateMachine.JumpingState);
    }
}
