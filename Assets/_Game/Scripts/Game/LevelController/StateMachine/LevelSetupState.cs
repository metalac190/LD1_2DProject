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

    private GameSession _gameSession;
    private PlayerSpawner _spawner;
    private IntroScreen _introScreen;

    public LevelSetupState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _spawner = controller.PlayerSpawner;
        _gameSession = GameSession.Instance;
        _introScreen = controller.LevelHUD.IntroScreen;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Setup");

        _introScreen.Display();
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

        if (_gameSession.IsFirstAttempt)
        {
            _gameSession.SpawnLocation = _spawner.StartSpawnLocation.position;
            _stateMachine.ChangeState(_stateMachine.IntroState);
            return;
        }
        else
        {
            Player player = _spawner.RespawnPlayer(_gameSession.SpawnLocation);
            _gameSession.LoadPlayerData(player);
            _stateMachine.ChangeState(_stateMachine.ActiveState);
            return;
        }
    }
}
