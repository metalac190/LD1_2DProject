using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeHangState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    Movement _movement;
    PlayerData _data;
    GameplayInput _input;
    LedgeDetector _ledgeDetector;
    PlayerAnimator _playerAnimator;
    PlayerSFXData _sfx;

    Vector2 _cornerPos;

    private Vector2 _hangPosition;


    public PlayerLedgeHangState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _movement = player.Actor.Movement;
        _data = player.Data;
        _input = player.Input;
        _ledgeDetector = player.Actor.CollisionDetector.LedgeDetector;
        _playerAnimator = player.PlayerAnimator;
        _sfx = player.SFX;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Ledge Hang");

        _input.JumpPressed += OnJumpPressed;
        _input.MovementPressed += OnMovementPressed;

        _playerAnimator.LedgeHangVisual.SetActive(true);

        _cornerPos = _ledgeDetector.CalculateUpperLedgeCornerPosition(_movement.FacingDirection);

        CalculateClimbPositions();
        // set initial player position
        _movement.MovePositionInstant(_hangPosition);
        _movement.HoldPosition(_hangPosition);

        _sfx.LedgeCatchSFX.PlayOneShot(_player.transform.position);
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

        _movement.HoldPosition(_hangPosition);
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
        else if (_input.YInputRaw < 0)
        {
            if (_data.AllowWallSlide && _input.XInputRaw == _movement.FacingDirection)
            {
                _stateMachine.ChangeState(_stateMachine.WallSlideState);
                return;
            }
            else
            {
                // pause briefly so we don't insta-regrab
                _ledgeDetector.Pause(.15f);
                _movement.SetVelocityY(-_data.LedgeDropPushVelocity);
                // start falling
                _stateMachine.ChangeState(_stateMachine.FallingState);
                return;
            }
        }
    }

    private void CalculateClimbPositions()
    {
        // calculate start climb from player offsets
        _hangPosition = new Vector2(_cornerPos.x - (_movement.FacingDirection * _data.StartClimbOffset.x),
            _cornerPos.y - _data.StartClimbOffset.y);
    }

    private void OnMovementPressed()
    {
        if(_input.MoveInput.y > 0)
            _stateMachine.ChangeState(_stateMachine.LedgeClimbState);
    }

    private void OnJumpPressed()
    {
        _stateMachine.ChangeState(_stateMachine.WallJumpState);
    }
}
