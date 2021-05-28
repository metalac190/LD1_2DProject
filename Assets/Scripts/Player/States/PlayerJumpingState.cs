using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : State
{
    PlayerFSM _stateMachine;
    Player _player;

    InputManager _input;
    PlayerData _data;
    GroundDetector _groundDetector;

    public PlayerJumpingState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;

        _input = player.Input;
        _data = player.Data;
        _groundDetector = player.GroundDetector;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Jump");

        _input.SpacebarReleased += OnSpacebarReleased;

        _player.DecreaseJumpsRemaining();
        _player.SetVelocityY(_data.JumpForce);

    }

    public override void Exit()
    {
        base.Exit();

        _input.SpacebarReleased -= OnSpacebarReleased;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if we're not grounded, but began falling, go to fall state
        if(!_groundDetector.IsGrounded && _player.RB.velocity.y <= 0)
        {
            _stateMachine.ChangeState(_stateMachine.FallingState);
        }
    }

    public override void Update()
    {
        base.Update();
        
        _player.SetVelocityX(_input.XRaw * _data.MoveSpeed);
    }

    private void OnSpacebarReleased()
    {
        // cut the jump short on release
        _player.SetVelocityY(_player.RB.velocity.y * _data.ShortJumpHeightMultiplier);
    }
}
