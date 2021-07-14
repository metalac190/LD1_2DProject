using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoseState : State
{
    private LevelFSM _stateMachine;

    private PlayerSpawner _playerSpawner;

    public LevelLoseState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _playerSpawner = controller.PlayerSpawner;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Lose state");
        //TODO save data on death, so we can determine respawn point
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(StateDuration >= _playerSpawner.RespawnDelay)
        {
            LevelLoader.ReloadLevel();
            return;
        }
    }
}
