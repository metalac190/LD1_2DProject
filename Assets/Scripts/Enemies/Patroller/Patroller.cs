using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(ReceiveKnockback))]
public class Patroller : Enemy
{
    [Header("Patroller Settings")]
    [SerializeField]
    private PatrollerData _data;
    [SerializeField]
    private PatrollerAnimator _patrollerAnimator;
    [SerializeField]
    private PlayerDetector _playerDetector;
    [SerializeField]
    private ReceiveKnockback _receiveKnockback;

    [SerializeField]
    private GameObject _detectedGraphic;
    [SerializeField]
    private GameObject _attackLocation;

    public PatrollerData Data => _data;
    public PatrollerAnimator PatrollerAnimator => _patrollerAnimator;
    public PlayerDetector PlayerDetector => _playerDetector;
    public ReceiveKnockback ReceiveKnockback => _receiveKnockback;

    public GameObject DetectedGraphic => _detectedGraphic;
    public GameObject AttackLocation => _attackLocation;

    private void Awake()
    {
        _attackLocation.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackLocation.transform.position, _data.AttackRadius);
    }
}
