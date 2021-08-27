using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiveState : State
{
    private LevelFSM _stateMachine;

    private WinTrigger _winTrigger;
    private PlayerSpawner _playerSpawner;

    private Player _activePlayer;
    private GameSession _gameSession;
    private PlaytimeScreen _playtimeScreen;

    private MenuInput _menuInput;

    private float _elapsedTime;


    public LevelActiveState(LevelFSM stateMachine, LevelController levelController)
    {
        _stateMachine = stateMachine;

        _winTrigger = levelController.WinTrigger;
        _playerSpawner = levelController.PlayerSpawner;
        _gameSession = GameSession.Instance;
        _playtimeScreen = levelController.LevelHUD.PlaytimeScreen;

        _menuInput = levelController.MenuInput;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("LEVEL: Active");
        _winTrigger.PlayerEntered += OnPlayerEnteredWin;
        _playerSpawner.PlayerRemoved += OnPlayerDied;

        _menuInput.CancelPressed += OnCancelPressed;
        // load elapsed time from data
        _elapsedTime = _gameSession.ElapsedTime;
    }

    public override void Exit()
    {
        base.Exit();

        _winTrigger.PlayerEntered -= OnPlayerEnteredWin;
        _playerSpawner.PlayerRemoved -= OnPlayerDied;
        _menuInput.CancelPressed -= OnCancelPressed;
        // save elapsed time to data
        _gameSession.ElapsedTime = _elapsedTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        _elapsedTime += Time.deltaTime;
        _playtimeScreen.IncrementPlaytimeDisplay(_elapsedTime);
    }



    private void OnPlayerEnteredWin()
    {
        _stateMachine.ChangeState(_stateMachine.WinState);
    }

    private void OnPlayerDied(Player player)
    {
        Debug.Log("Player DIED!");
        player.PlayDeathFX();
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
