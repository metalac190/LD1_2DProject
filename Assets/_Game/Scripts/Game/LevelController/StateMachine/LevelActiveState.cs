using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiveState : State
{
    private LevelFSM _stateMachine;

    private WinTrigger _winTrigger;
    private PlayerSpawner _playerSpawner;

    private Player _activePlayer;

    public LevelActiveState(LevelFSM stateMachine, LevelController levelController)
    {
        _stateMachine = stateMachine;

        _winTrigger = levelController.WinTrigger;
        _playerSpawner = levelController.PlayerSpawner;
    }

    public override void Enter()
    {
        base.Enter();

        _winTrigger.PlayerEntered += OnPlayerEnteredWin;
        _playerSpawner.PlayerDied += OnPlayerDied;
    }

    public override void Exit()
    {
        base.Exit();

        _winTrigger.PlayerEntered -= OnPlayerEnteredWin;
        _playerSpawner.PlayerDied -= OnPlayerDied;
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
        _stateMachine.ChangeState(_stateMachine.LoseState);
    }
}
