using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private PlayerSpawner _playerSpawner;
    [SerializeField]
    private GameplayInput _gameplayInput;
    [SerializeField]
    private MenuInput _menuInput;
    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;
    [SerializeField]
    private WinTrigger _winTrigger;
    [SerializeField]
    private LevelHUD _levelHUD;

    public LevelData LevelData => _levelData;
    public PlayerSpawner PlayerSpawner => _playerSpawner;
    public GameplayInput GameplayInput => _gameplayInput;
    public MenuInput MenuInput => _menuInput;
    public CinemachineVirtualCamera MainCamera => _playerCamera;
    public WinTrigger WinTrigger => _winTrigger;
    public LevelHUD LevelHUD => _levelHUD;
} 
