using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : StateMachineMB
{
    [SerializeField]
    private Player _player;
    // grounded
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    // air
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    // wall
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrab WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }

    private void Awake()
    {
        // grounded
        IdleState = new PlayerIdleState(this, _player);
        MoveState = new PlayerMoveState(this, _player);
        // air
        JumpingState = new PlayerJumpingState(this, _player);
        FallingState = new PlayerFallingState(this, _player);
        LandState = new PlayerLandState(this, _player);
        // wall
        WallSlideState = new PlayerWallSlideState(this, _player);
        WallGrabState = new PlayerWallGrab(this, _player);
        WallClimbState = new PlayerWallClimbState(this, _player);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
