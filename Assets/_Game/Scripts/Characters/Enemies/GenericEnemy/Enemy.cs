using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Enemy Dependencies")]
    [SerializeField]
    private PlayerDetector _playerDetector;
    [SerializeField]
    private HitVolume _hitVolume;

    public PlayerDetector PlayerDetector => _playerDetector;
    public HitVolume HitVolume => _hitVolume;
}
