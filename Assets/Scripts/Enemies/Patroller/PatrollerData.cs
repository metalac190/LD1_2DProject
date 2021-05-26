using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrollerData_", menuName = "Data/Enemies/Patroller")]
public class PatrollerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField][Tooltip("Speed while walking")]
    private float _movementSpeed = 3f;
    [SerializeField][Tooltip("Whether or not patroller stops at a wall or ledge")]
    private bool _idleOnPathEnd = true;
    [SerializeField][Tooltip("Min time patroller will wait")]
    private float _minIdleTime = 1;
    [SerializeField][Tooltip("Max time patroller will wait")]
    private float _maxIdleTime = 2;

    [Header("Charging")]
    [SerializeField][Tooltip("Speed when charging at the player")]
    private float _chargeSpeed = 8;
    [SerializeField][Tooltip("Duration of charge")]
    private float _chargeDuratoin = 1;

    [Header("Searching")]
    [SerializeField][Tooltip("Duration of brief pause when player is detected")]
    private float _detectedPauseTime = 0.35f;
    [SerializeField][Tooltip("Should first turn happen instantly")]
    private bool _turnImmediatelyOnSearch = false;
    [SerializeField][Tooltip("Number of times patroller will look back and forth while searching")]
    private int _numberOfSearchTurns = 2;
    [SerializeField][Tooltip("Duration of each turn before flipping the opposite direction")]
    private float _searchTurnDuration = .75f;

    [Header("Attacking")]
    [SerializeField][Tooltip("Damage dealt by melee attack")]
    private int _attackDamage = 20;
    [SerializeField][Tooltip("Duration of pause before attack becomes active")]
    private float _attackStartupDuration = .35f;
    [SerializeField][Tooltip("Duration that melee attack is active and harms player")]
    private float _attackActiveDuration = 0.5f;
    [SerializeField][Tooltip("Duration of wait after attack has happened before transitioning states")]
    private float _attackAfterDuration = .35f;
    [SerializeField][Tooltip("Size of melee attack")]
    private float _attackRadius = 0.5f;
    [SerializeField][Tooltip("Physics layers to detect colliders for attack")]
    private LayerMask _attackableLayers;

    [Header("Death")]
    [SerializeField]
    private float _deathTimeDuration = 0.5f;

    // movement
    public float MovementSpeed => _movementSpeed;
    public float MinIdleTime => _minIdleTime;
    public float MaxIdleTime => _maxIdleTime;
    public bool IdleOnPathEnd => _idleOnPathEnd;

    // charging
    public float ChargeSpeed => _chargeSpeed;
    public float ChargeDuration => _chargeDuratoin;

    // searching
    public float DetectedPauseTime => _detectedPauseTime;
    public bool TurnImmediatelyOnSearch => _turnImmediatelyOnSearch;
    public int NumberOfSearchTurns => _numberOfSearchTurns;
    public float SearchTurnDuration => _searchTurnDuration;

    // attacking
    public int AttackDamage => _attackDamage;
    public float AttackActiveDuration => _attackActiveDuration;
    public float AttackStartupDuration => _attackStartupDuration;
    public float AttackAfterDuration => _attackAfterDuration;
    public float AttackRadius => _attackRadius;
    public LayerMask AttackableLayers => _attackableLayers;

    // death
    public float DeathTimeDuration => _deathTimeDuration;
}
