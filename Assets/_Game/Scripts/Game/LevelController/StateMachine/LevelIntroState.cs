using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIntroState : State
{
    private LevelFSM _stateMachine;

    private MenuInput _input;
    private HUDScreen _introScreen;
    private PlayerSpawner _playerSpawner;

    public LevelIntroState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _input = controller.MenuInput;
        _introScreen = controller.LevelHUD.IntroScreen;
        _playerSpawner = controller.PlayerSpawner;
    }

    public override void Enter()
    {
        base.Enter();
        _input.SubmitPressed += OnSubmitPressed;
        // spawn player if a player doesn't already exist
        _introScreen.Display();
    }

    public override void Exit()
    {
        base.Exit();

        _playerSpawner.SpawnPlayer();

        _input.SubmitPressed -= OnSubmitPressed;
        _introScreen.Hide();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnSubmitPressed()
    {
        // immediately transition to active play state
        _stateMachine.ChangeState(_stateMachine.ActiveState);
    }
}
