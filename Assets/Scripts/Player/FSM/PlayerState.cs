using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player Player;
    protected PlayerFSM StateMachine;
    protected InputManager Input;

    public PlayerState(Player player, PlayerFSM stateMachine, InputManager input)
    {
        this.Player = player;
        this.StateMachine = stateMachine;
        this.Input = input;
    }
}
