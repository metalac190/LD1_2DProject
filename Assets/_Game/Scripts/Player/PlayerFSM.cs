using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerFSM : StateMachineMB
{
    private Player _player;
    // grounded
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    // air
    public PlayerAirJumpState AirJumpState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    // wall
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrab WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    // ledge
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    // abilities
    public PlayerDashState DashState { get; private set; }
    // attacks
    public PlayerGroundAttackState GroundAttackState { get; private set; }
    public PlayerAirAttackState AirAttackState { get; private set; }
    public PlayerWallAttackState WallAttackState { get; private set; }
    public PlayerBounceAttackState BounceAttackState { get; private set; }

    private void Awake()
    {
        _player = GetComponent<Player>();
        // grounded
        IdleState = new PlayerIdleState(this, _player);
        MoveState = new PlayerMoveState(this, _player);
        JumpState = new PlayerJumpState(this, _player);
        LandState = new PlayerLandState(this, _player);
        CrouchState = new PlayerCrouchState(this, _player);
        CrouchMoveState = new PlayerCrouchMoveState(this, _player);
        // air
        AirJumpState = new PlayerAirJumpState(this, _player);
        FallingState = new PlayerFallingState(this, _player);
        // wall
        WallSlideState = new PlayerWallSlideState(this, _player);
        WallGrabState = new PlayerWallGrab(this, _player);
        WallClimbState = new PlayerWallClimbState(this, _player);
        WallJumpState = new PlayerWallJumpState(this, _player);
        // ledge
        LedgeClimbState = new PlayerLedgeClimbState(this, _player);
        // abilities
        DashState = new PlayerDashState(this, _player);
        // attacks
        GroundAttackState = new PlayerGroundAttackState(this, _player);
        AirAttackState = new PlayerAirAttackState(this, _player);
        WallAttackState = new PlayerWallAttackState(this, _player);
        BounceAttackState = new PlayerBounceAttackState(this, _player);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
