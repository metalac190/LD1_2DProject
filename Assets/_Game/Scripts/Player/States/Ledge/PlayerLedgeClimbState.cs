using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    LedgeDetector _ledgeDetector;
    Rigidbody2D _rb;

    private Vector2 _cornerPos;
    private Vector2 _hangPosition;
    private Vector2 _stopClimbPos;

    public PlayerLedgeClimbState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _ledgeDetector = player.LedgeDetector;
        _rb = player.RB;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Ledge Climb");

        _cornerPos = _ledgeDetector.CalculateUpperLedgeCornerPosition(_player.FacingDirection);

        CalculateClimbPositions();
        // set initial player position
        _rb.MovePosition(_hangPosition);
        _player.HoldPosition(_hangPosition);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _player.HoldPosition(_hangPosition);
    }

    public override void Update()
    {
        base.Update();

        // once climb duration is completed, move on top of ledge
        if(StateDuration >= _data.LedgeClimbDuration)
        {
            Debug.Log("Climb!");
            _rb.position = _stopClimbPos;
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void CalculateClimbPositions()
    {
        // calculate start climb from player offsets
        _hangPosition = new Vector2(_cornerPos.x - (_player.FacingDirection * _data.StartClimbOffset.x),
            _cornerPos.y - _data.StartClimbOffset.y);
        // calculate stop climb from player offsets
        _stopClimbPos = new Vector2(_cornerPos.x + (_player.FacingDirection * _data.StopClimbOffset.x),
            _cornerPos.y + _data.StopClimbOffset.y);
    }
}
