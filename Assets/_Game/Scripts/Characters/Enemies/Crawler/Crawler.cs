using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    [Header("Crawler Data")]
    [SerializeField]
    private float _movementSpeed = 3f;
    [SerializeField]
    private bool _reverseAtLedge = true;

    public float MovementSpeed => _movementSpeed;
    public bool ReverseAtLedge => _reverseAtLedge;

    [Header("Dependencies")]
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private HitVolume _damageZone;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private ReceiveHit _receiveHit;
    [SerializeField]
    private OverlapDetector _wallDetector;
    [SerializeField]
    private OverlapDetector _groundDetector;
    [SerializeField]
    private OverlapDetector _groundInFrontDetector;

    public MovementKM Movement => _movement;
    public HitVolume DamageZone => _damageZone;
    public Health Health => _health;
    public ReceiveHit ReceiveHit => _receiveHit;
    public OverlapDetector WallDetector => _wallDetector;
    public OverlapDetector GroundDetector => _groundDetector;
    public OverlapDetector GroundInFrontDetector => _groundInFrontDetector;
}
