using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    [Header("Crawler")]
    [SerializeField]
    private CrawlerData _data;
    [SerializeField]
    private MovementKM _movement;
    [SerializeField]
    private DamageZone _damageZone;
    [SerializeField]
    private WallDetector _wallDetector;
    [SerializeField]
    private LedgeDetector _ledgeDetector;

    public CrawlerData Data => _data;
    public MovementKM Movement => _movement;
    public DamageZone DamageZone => _damageZone;
    public WallDetector WallDetector => _wallDetector;
    public LedgeDetector LedgeDetector => _ledgeDetector;
}