using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeHangState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    PlayerData _data;
    GameplayInput _input;
    LedgeDetector _ledgeDetector;
    PlayerAnimator _playerAnimator;

    Vector2 _cornerPos;

    private Vector2 _hangPosition;

    public PlayerLedgeHangState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _data = player.Data;
        _input = player.Input;
        _ledgeDetector = player.LedgeDetector;
        _playerAnimator = player.PlayerAnimator;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Ledge Climb");

        _input.JumpPressed += OnJumpPressed;
        _input.MovementPressed += OnMovementPressed;

        _playerAnimator.LedgeHangVisual.SetActive(true);

        _cornerPos = _ledgeDetector.CalculateUpperLedgeCornerPosition(_player.FacingDirection);

        CalculateClimbPositions();
        // set initial player position
        _player.RB.MovePosition(_hangPosition);
        _player.HoldPosition(_hangPosition);
    }

    public override void Exit()
    {
        base.Exit();

        _input.JumpPressed -= OnJumpPressed;
        _input.MovementPressed -= OnMovementPressed;

        _playerAnimator.LedgeHangVisual.SetActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _player.HoldPosition(_hangPosition);
    }

    public override void Update()
    {
        base.Update();
        // if we press down, drop 
        if (_data.ShouldAutoClimb)
        {
            _stateMachine.ChangeState(_stateMachine.LedgeClimbState);
            return;
        }
        // otherwise wait for input
        else if (_input.YRaw < 0)
        {
            if (_data.AllowWallSlide && _input.XRaw == _player.FacingDirection)
            {
                _stateMachine.ChangeState(_stateMachine.WallSlideState);
                return;
            }
            else
            {
                // pause briefly so we don't insta-regrab
                _ledgeDetector.Pause(.15f);
                _player.SetVelocityY(-_data.LedgeDropPushVelocity);
                // start falling
                _stateMachine.ChangeState(_stateMachine.FallingState);
                return;
            }
        }
    }

    private void CalculateClimbPositions()
    {
        // calculate start climb from player offsets
        _hangPosition = new Vector2(_cornerPos.x - (_player.FacingDirection * _data.StartClimbOffset.x),
            _cornerPos.y - _data.StartClimbOffset.y);
    }

    private void OnMovementPressed()
    {
        if(_input.Movement.y > 0)
            _stateMachine.ChangeState(_stateMachine.LedgeClimbState);
    }

    private void OnJumpPressed()
    {
        _stateMachine.ChangeState(_stateMachine.WallJumpState);
    }
}
