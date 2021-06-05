using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private float _respawnDelay = 1.5f;

    private bool _isRespawning = false;

    private GameObject _currentPlayer;
    private Coroutine _respawnRoutine;

    private void OnEnable()
    {
        //_playerHealth.Died.AddListener(Respawn);
    }

    private void OnDisable()
    {
        //_playerHealth.Died.RemoveListener(Respawn);
    }

    public void Respawn()
    {
        Debug.Log("Respawning...");
        if (_isRespawning) { return; }

        _respawnRoutine = StartCoroutine(RespawnRoutine(_respawnDelay));
    }

    IEnumerator RespawnRoutine(float delayBeforeSpawn)
    {
        _isRespawning = true;
        

        yield return new WaitForSeconds(delayBeforeSpawn);

        Spawn();

        _isRespawning = false;
    }

    private void Spawn()
    {
        _currentPlayer = Instantiate(_playerPrefab, _respawnPoint);
        //_followCam.SetNewTarget(_currentPlayer.transform);
    }
} 
