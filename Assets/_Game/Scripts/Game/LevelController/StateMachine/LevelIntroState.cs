using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: The difference between 'setup' and 'intro', is setup will always get calledon level load.
/// Intro is only called if it's determined that this is the player's first experience with the level.
/// (cutscenes, initialization, etc.)
/// </summary>
public class LevelIntroState : State
{
    private LevelFSM _stateMachine;

    private MenuInput _input;
    private PlayerSpawner _playerSpawner;
    private GameSession _gameSession;

    public LevelIntroState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _input = controller.MenuInput;
        _playerSpawner = controller.PlayerSpawner;
        _gameSession = GameSession.Instance;
    }

    public override void Enter()
    {
        base.Enter();

        _input.SubmitPressed += OnSubmitPressed;

    }

    public override void Exit()
    {
        base.Exit();

        _input.SubmitPressed -= OnSubmitPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        // check if our cutscene is complete, if we have one. For now, just skip
        BeginLevel();
    }

    // basically, this allows us to skip the cutscene
    private void OnSubmitPressed()
    {
        BeginLevel();
    }

    private void BeginLevel()
    {

        Player player = _playerSpawner.SpawnPlayer(_gameSession.SpawnLocation);
        _gameSession.LoadPlayerData(player);
        _stateMachine.ChangeState(_stateMachine.ActiveState);
    }
}
