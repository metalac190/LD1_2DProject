using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
public class Patroller : EnemyOLD
{
    [Header("Patroller Settings")]
    [SerializeField]
    private PatrollerData _data;
    [SerializeField]
    private PatrollerAnimator _patrollerAnimator;
    [SerializeField]
    private PlayerDetector _playerDetector;

    [SerializeField]
    private GameObject _detectedGraphic;
    [SerializeField]
    private GameObject _attackLocation;

    public PatrollerData Data => _data;
    public PatrollerAnimator PatrollerAnimator => _patrollerAnimator;
    public PlayerDetector PlayerDetector => _playerDetector;

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
