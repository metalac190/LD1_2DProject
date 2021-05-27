using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : StateMachineMB
{
    [SerializeField]
    private Player _player;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    private void Awake()
    {
        IdleState = new PlayerIdleState(this, _player);
        MoveState = new PlayerMoveState(this, _player);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
