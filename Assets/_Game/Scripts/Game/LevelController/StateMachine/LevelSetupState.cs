using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: The difference between 'setup' and 'intro', is setup will always get calledon level load.
/// Intro is only called if it's determined that this is the player's first experience with the level.
/// (cutscenes, initialization, etc.)
/// </summary>
public class LevelSetupState : State
{
    private LevelFSM _stateMachine;

    private GameSessionData _gameSessionData;
    private PlayerSpawner _spawner;

    public LevelSetupState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _spawner = controller.PlayerSpawner;
        _gameSessionData = controller.GameSessionData;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Setup");
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

        if (_gameSessionData.IsFirstAttempt)
        {
            _gameSessionData.SpawnLocation = _spawner.StartSpawnLocation.position;
            _stateMachine.ChangeState(_stateMachine.IntroState);
            return;
        }
        else
        {
            _spawner.RespawnPlayer(_gameSessionData.SpawnLocation);
            _stateMachine.ChangeState(_stateMachine.ActiveState);
            return;
        }
    }
}
