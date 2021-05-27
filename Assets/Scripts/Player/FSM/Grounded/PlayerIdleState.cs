using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    PlayerFSM _stateMachine;
    Player _player;
    InputManager _input;

    public PlayerIdleState(PlayerFSM stateMachine, Player player) 
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Idle");

        _player.SetVelocityX(0);
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
        Debug.Log("Input: " + _input.XRaw);
        if(_input.XRaw != 0)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }
}
