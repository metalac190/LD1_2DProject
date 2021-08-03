using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpawner : MonoBehaviour
{
    public event Action<Player> PlayerSpawned;
    public event Action<Player> PlayerRemoved;

    [SerializeField]
    private LevelController _levelController;

    [Header("Player Spawning")]

    [SerializeField]
    private Transform _startSpawnLocation;
    [SerializeField]
    private float _respawnDelay = 1.5f;
    [SerializeField]
    private Player _playerPrefab;

    private Player _player;

    public float RespawnDelay => _respawnDelay;
    public Player ActivePlayer => _player;
    public Transform StartSpawnLocation => _startSpawnLocation;

    /// <summary>
    /// Respawn player at designated location
    /// </summary>
    /// <param name="respawnPosition"></param>
    public Player RespawnPlayer(Vector3 respawnPosition)
    {
        // if there's already a player, remove it
        if (_player != null)
        {
            RemoveExistingPlayer();
        }

        _player = Instantiate(_playerPrefab, respawnPosition, Quaternion.identity);
        //TODO look into a way to pass this information before instantiating (it calls awake before initialize)
        _player.Initialize(_levelController.GameplayInput);
        _player.Health.Died.AddListener(OnPlayerDied);

        PlayerSpawned?.Invoke(_player);

        _levelController.MainCamera.Follow = _player.transform;

        return _player;
    }

    /// <summary>
    /// Spawn a new player at start position
    /// </summary>
    public Player SpawnPlayer()
    {
        // if there's already a player, remove it
        if(_player != null)
        {
            RemoveExistingPlayer();
        }

        _player = Instantiate(_playerPrefab, _startSpawnLocation.position, Quaternion.identity);
        //TODO look into a way to pass this information before instantiating (it calls awake before initialize)
        _player.Initialize(_levelController.GameplayInput);
        _player.Health.Died.AddListener(OnPlayerDied);

        PlayerSpawned?.Invoke(_player);

        _levelController.MainCamera.Follow = _player.transform;

        return _player;
    }

    public void RemoveExistingPlayer()
    {
        PlayerRemoved?.Invoke(_player);
        Destroy(_player.gameObject);
    }

    private void OnPlayerDied()
    {
        _player.Health.Died.RemoveListener(OnPlayerDied);
        RemoveExistingPlayer();
    }

}
