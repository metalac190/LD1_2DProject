using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpawner : MonoBehaviour
{
    public event Action<Player> PlayerSpawned;
    public event Action<Player> PlayerDied;

    [SerializeField]
    private LevelController _levelController;

    [Header("Player Spawning")]

    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private float _respawnDelay = 1.5f;
    [SerializeField]
    private Player _playerPrefab;

    private Player _player;

    public float RespawnDelay => _respawnDelay;
    public Player ActivePlayer => _player;

    public void SpawnPlayer()
    {
        RemoveExistingPlayer();

        _player = Instantiate(_playerPrefab, _spawnLocation.position, _spawnLocation.rotation);
        Debug.Log("Player Initialize");
        _player.Initialize(_levelController.GameplayInput);
        _player.Health.Died.AddListener(OnPlayerDied);

        PlayerSpawned?.Invoke(_player);

        _levelController.MainCamera.Follow = _player.transform;
    }

    public void RemoveExistingPlayer()
    {
        if (_player != null)
        {
            PlayerDied?.Invoke(_player);
            Destroy(_player.gameObject);
        }
    }

    private void OnPlayerDied()
    {
        _player.Health.Died.RemoveListener(OnPlayerDied);
        PlayerDied?.Invoke(_player);
    }
}
