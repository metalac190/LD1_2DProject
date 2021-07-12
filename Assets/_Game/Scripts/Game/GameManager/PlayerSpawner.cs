using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private LevelController _levelController;

    [Header("Player Spawning")]
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private float _respawnDelay = 1.5f;
    [SerializeField]
    private Player _playerPrefab;

    public float RespawnDelay => _respawnDelay;
    public Player ActivePlayer => _player;

    private void Awake()
    {
        // ensure we have an active player
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                SpawnPlayer();
            }
        }
    }

    private void SpawnPlayer()
    {
        if (_player != null)
            Destroy(_player.gameObject);

        _player = Instantiate(_playerPrefab, _spawnLocation.position, _spawnLocation.rotation);
        _player.Initialize(_levelController.GameplayInput);

        _levelController.MainCamera.Follow = _player.transform;
    }
}
