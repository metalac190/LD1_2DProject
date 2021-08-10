using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : State
{
    PlayerFSM _stateMachine;

    MovementKM _movement;
    PlayerMoveData _data;
    PlayerSFXData _sfx;

    float _hopDuration = .25f;

    public PlayerLedgeClimbState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;

        _movement = player.Movement;
        _data = player.Data;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Ledge Climb");

        _movement.MoveY(_data.LedgeHopAmount);
        _sfx.LedgeCatchSFX.PlayOneShot(_movement.Position);
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

        if(StateDuration >= _hopDuration)
        {
            Debug.Log("Fall state");
            _stateMachine.ChangeState(_stateMachine.FallingState);
            return;
        }
    }
}
