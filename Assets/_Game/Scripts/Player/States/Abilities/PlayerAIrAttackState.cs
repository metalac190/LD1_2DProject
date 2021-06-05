using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIrAttackState : State
{
    private PlayerFSM _stateMachine;
    private Player _player;

    public PlayerAIrAttackState(PlayerFSM stateMachine, Player player)
    {
        _stateMachine = stateMachine;
        _player = player;
    }
}
