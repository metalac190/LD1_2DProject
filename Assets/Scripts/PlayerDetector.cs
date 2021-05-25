using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This component casts rays in the forward direction to look for the player
/// </summary>
public class PlayerDetector : MonoBehaviour
{
    public UnityEvent PlayerDetected;
    public UnityEvent PlayerEscaped;

    private bool _isPlayerInStartAggroRange = false;
    private bool _isPlayerInDetectedAggroRange = false;

    public bool IsPlayerDetected { get; private set; } = false;

    [Header("Player Detection")]
    [SerializeField]
    private float _startAggroDistance = 2;
    [SerializeField]
    private float _detectedAggroDistance = 4;
    [SerializeField]
    private LayerMask _whatIsPlayer;
    [SerializeField]
    private Transform _playerCheck;
    [SerializeField]
    private float _checkFrequency = .2f;
    [SerializeField]
    private bool _startCheckingOnAwake = true;

    private Coroutine _playerCheckRoutine;

    private void Start()
    {
        if (_startCheckingOnAwake)
        {
            StartCheckingForPlayer();
        }
    }

    public void StartCheckingForPlayer()
    {
        if (_playerCheckRoutine != null)
            StopCoroutine(_playerCheckRoutine);
        _playerCheckRoutine = StartCoroutine(CheckForPlayerRoutine(_checkFrequency));
    }

    public void StopCheckingForPlayer()
    {
        if (_playerCheckRoutine != null)
            StopCoroutine(_playerCheckRoutine);
    }

    private IEnumerator CheckForPlayerRoutine(float frequency)
    {
        while (true)
        {
            // if player is already detected, check to see if they've escaped
            if (IsPlayerDetected)
            {
                _isPlayerInDetectedAggroRange = CheckPlayerInDetectedAggroRange();
                if (_isPlayerInDetectedAggroRange == false)
                {
                    IsPlayerDetected = false;
                    PlayerEscaped?.Invoke();
                }
            }
            // otherwise look for them
            else
            {
                _isPlayerInStartAggroRange = CheckPlayerInStartAggroRange();
                if (_isPlayerInStartAggroRange)
                {
                    IsPlayerDetected = true;
                    PlayerDetected?.Invoke();
                }
            }

            yield return new WaitForSeconds(frequency);
        }

    }

    public bool CheckPlayerInStartAggroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right,
            _startAggroDistance, _whatIsPlayer);
    }

    public bool CheckPlayerInDetectedAggroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right,
            _detectedAggroDistance, _whatIsPlayer);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(_playerCheck.position, _playerCheck.position 
            + (transform.right * _startAggroDistance));
    }
}
