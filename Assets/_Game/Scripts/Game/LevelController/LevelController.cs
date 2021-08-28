using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private WinTrigger _winTrigger;
    [SerializeField]
    private LevelHUD _levelHUD;
    [SerializeField]
    private CameraController _cameraController;

    public LevelData LevelData => _levelData;
    public PlayerSpawner PlayerSpawner => _playerSpawner;
    public GameplayInput GameplayInput => _gameplayInput;
    public MenuInput MenuInput => _menuInput;
    public WinTrigger WinTrigger => _winTrigger;
    public LevelHUD LevelHUD => _levelHUD;
    public CameraController CameraController => _cameraController;
} 
