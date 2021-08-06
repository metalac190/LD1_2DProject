using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{
    [Header("Flyer Data")]
    [SerializeField]
    private float _chaseSpeed = 5;
    [SerializeField]
    private float _returnSpeed = 8;

    [Header("Dependencies")]
    [SerializeField]
    private OverlapDetector _playerDetector; // this will detect if player is near
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private ReceiveHit _receiveHit;
    [SerializeField]
    private HitVolume _hitVolume;

    public Vector3 StartPosition { get; private set; }

    public float ChaseSpeed => _chaseSpeed;
    public float ReturnSpeed => _returnSpeed;

    public OverlapDetector PlayerDetector => _playerDetector;
    public MovementKM Movement => _movement;
    public ReceiveHit ReceiveHit => _receiveHit;
    public HitVolume HitVolume => _hitVolume;

    private void Awake()
    {
        StartPosition = transform.position;
    }
}
