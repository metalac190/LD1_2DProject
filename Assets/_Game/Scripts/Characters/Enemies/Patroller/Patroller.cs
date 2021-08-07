using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    [Header("Patroller Settings")]
    [SerializeField]
    private PatrollerData _data;

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private ReceiveHit _receiveHit;
    [SerializeField]
    private PatrollerAnimator _patrollerAnimator;
    [SerializeField]
    private GameObject _detectedGraphic;

    [Header("Environment Detection")]
    [SerializeField]
    private OverlapDetector _groundDetector;
    [SerializeField]
    private OverlapDetector _wallDetector;
    [SerializeField]
    private OverlapDetector _farGroundDetector;
    [SerializeField]
    private OverlapDetector _closeRangeDetector;
    [SerializeField]
    private HitVolume _hitVolume;
    [SerializeField]
    private RayDetector _aggroDetector;

    public Rigidbody2D RB => _rb;
    public MovementKM Movement => _movement;
    public Health Health => _health;
    public ReceiveHit ReceiveHit => _receiveHit;
    public PatrollerData Data => _data;
    public PatrollerAnimator PatrollerAnimator => _patrollerAnimator;
    public HitVolume HitVolume => _hitVolume;
    public GameObject DetectedGraphic => _detectedGraphic;

    public OverlapDetector GroundDetector => _groundDetector;
    public OverlapDetector WallDetector => _wallDetector;
    public OverlapDetector GroundInFrontDetector => _farGroundDetector;
    public OverlapDetector CloseRangeDetector => _closeRangeDetector;
    public RayDetector AggroDetector => _aggroDetector;

    private void Awake()
    {
        HitVolume.gameObject.SetActive(false);
        DetectedGraphic.gameObject.SetActive(false);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
