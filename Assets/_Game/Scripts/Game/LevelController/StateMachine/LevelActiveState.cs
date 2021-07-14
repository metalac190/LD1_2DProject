using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiveState : State
{
    private LevelFSM _stateMachine;

    private WinTrigger _winTrigger;
    private PlayerSpawner _playerSpawner;

    private Player _activePlayer;
    private GameSessionData _gameSession;

    private MenuInput _menuInput;

    public LevelActiveState(LevelFSM stateMachine, LevelController levelController)
    {
        _stateMachine = stateMachine;

        _winTrigger = levelController.WinTrigger;
        _playerSpawner = levelController.PlayerSpawner;
        _gameSession = levelController.GameSessionData;

        _menuInput = levelController.MenuInput;
    }

    public override void Enter()
    {
        base.Enter();

        _winTrigger.PlayerEntered += OnPlayerEnteredWin;
        _playerSpawner.PlayerRemoved += OnPlayerDied;

        _menuInput.CancelPressed += OnCancelPressed;
    }

    public override void Exit()
    {
        base.Exit();

        _winTrigger.PlayerEntered -= OnPlayerEnteredWin;
        _playerSpawner.PlayerRemoved -= OnPlayerDied;
        _menuInput.CancelPressed -= OnCancelPressed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnPlayerEnteredWin()
    {
        _stateMachine.ChangeState(_stateMachine.WinState);
    }

    private void OnPlayerDied(Player player)
    {
        Debug.Log("Player DIED!");
        _gameSession.DeathCount++;
        _stateMachine.ChangeState(_stateMachine.LoseState);
    }

    private void OnCancelPressed()
    {
        // reset level data. Make this clear to player in the future, and consider putting in menus
        _gameSession.ClearGameSession();
        LevelLoader.ReloadLevel();
    }
}
