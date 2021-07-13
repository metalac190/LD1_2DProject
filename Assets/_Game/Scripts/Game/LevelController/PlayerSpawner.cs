using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
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
        if (_player != null)
            Destroy(_player.gameObject);

        _player = Instantiate(_playerPrefab, _spawnLocation.position, _spawnLocation.rotation);
        _player.Initialize(_levelController.GameplayInput);

        _levelController.MainCamera.Follow = _player.transform;
    }
}
