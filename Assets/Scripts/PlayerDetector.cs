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

    private bool _isPlayerInAggroRange = false;
    private bool _isPlayerInEscapeRange = false;

    public bool IsPlayerInCloseRange { get; private set; } = false;
    public bool IsPlayerDetected { get; private set; } = false;

    [Header("Player Detection")]
    [SerializeField]
    private float _closeRangeDistance = 3;
    [SerializeField]
    private float _aggroRangeDistance = 8;
    [SerializeField]
    private float _escapeRangeDistance = 12;
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
        Debug.Log("Checking for player");
        while (true)
        {
            // if player is already detected, check to see if they've escaped
            if (IsPlayerDetected)
            {
                // make sure they haven't escaped
                _isPlayerInEscapeRange = CheckPlayerInEscapeRange();
                if (_isPlayerInEscapeRange == false)
                {
                    IsPlayerDetected = false;
                    IsPlayerInCloseRange = false;
                    PlayerEscaped?.Invoke();
                }
                // otherwise check player close range
                else
                {
                    IsPlayerInCloseRange = CheckPlayerInCloseRange();
                }
            }
            // otherwise look for them
            else
            {
                _isPlayerInAggroRange = CheckPlayerInAggroRange();
                if (_isPlayerInAggroRange)
                {
                    IsPlayerDetected = true;
                    PlayerDetected?.Invoke();
                }
            }

            yield return new WaitForSeconds(frequency);
        }

    }

    public bool CheckPlayerInCloseRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right,
            _closeRangeDistance, _whatIsPlayer);
    }

    public bool CheckPlayerInAggroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right,
            _aggroRangeDistance, _whatIsPlayer);
    }

    public bool CheckPlayerInEscapeRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right,
            _escapeRangeDistance, _whatIsPlayer);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_playerCheck.position, _playerCheck.position
            + (transform.right * _escapeRangeDistance));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_playerCheck.position, _playerCheck.position
            + (transform.right * _aggroRangeDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_playerCheck.position, _playerCheck.position
            + (transform.right * _closeRangeDistance));
    }
}
