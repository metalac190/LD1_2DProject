using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWinState : State
{
    private LevelFSM _stateMachine;

    private HUDScreen _winScreen;
    private PlayerSpawner _playerSpawner;
    private MenuInput _input;
    private GameSession _gameSession;

    public LevelWinState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _playerSpawner = controller.PlayerSpawner;
        _winScreen = controller.LevelHUD.WinScreen;
        _input = controller.MenuInput;
        _gameSession = GameSession.Instance;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Win!");
        _input.CancelPressed += OnCancelPressed;
        _input.ClosePressed += OnClosePressed;
        
        //TODO save player stats before removing
        _winScreen.Display();

        //TODO optionally, we could create a 'PlayerInactive' state that doesn't take input,
        // in the meantime just remove it for simplicity
        _playerSpawner.RemoveExistingPlayer();
    }

    public override void Exit()
    {
        base.Exit();

        _input.CancelPressed -= OnCancelPressed;
        _input.ClosePressed -= OnClosePressed;


        _winScreen.Hide();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnCancelPressed()
    {
        _gameSession.ClearGameSession();
        LevelLoader.ReloadLevel();
    }

    private void OnClosePressed()
    {
        Application.Quit();
    }
}
