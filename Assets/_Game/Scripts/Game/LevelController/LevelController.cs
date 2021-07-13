using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private PlayerSpawner _playerSpawner;
    [SerializeField]
    private GameplayInput _gameplayInput;
    [SerializeField]
    private MenuInput _menuInput;
    [SerializeField]
    private CinemachineVirtualCamera _mainCamera;
    [SerializeField]
    private WinTrigger _winTrigger;
    [SerializeField]
    private LevelHUD _levelHUD;

    public PlayerSpawner PlayerSpawner => _playerSpawner;
    public GameplayInput GameplayInput => _gameplayInput;
    public MenuInput MenuInput => _menuInput;
    public CinemachineVirtualCamera MainCamera => _mainCamera;
    public WinTrigger WinTrigger => _winTrigger;
    public LevelHUD LevelHUD => _levelHUD;
} 
