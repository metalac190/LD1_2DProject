using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : StateMachineMB
{
    [SerializeField]
    private Player _player;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    private void Awake()
    {
        IdleState = new PlayerIdleState(this, _player);
        MoveState = new PlayerMoveState(this, _player);
        JumpingState = new PlayerJumpingState(this, _player);
        FallingState = new PlayerFallingState(this, _player);
        LandState = new PlayerLandState(this, _player);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
