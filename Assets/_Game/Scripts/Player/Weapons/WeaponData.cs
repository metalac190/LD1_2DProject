using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "SO_WeaponData_", menuName = "Data/Weapon/Melee Weapon")]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    private SFXOneShot _attackSFX;

    [Header("Ground Settings")]
    [SerializeField]
    private int _damageAmount = 5;
    [SerializeField]
    private float _startDelay = 0;
    [SerializeField]
    private float _activeDuration = .1f;
    [SerializeField]
    private float _endDelay = .1f;
    [SerializeField]
    private float _groundForwardAmount = 5;

    [Header("Air Settings")]
    [SerializeField]
    private float _airForwardAmount = 0;

    public int DamageAmount => _damageAmount;
    public float StartDelay => _startDelay;
    public float ActiveDuration => _activeDuration;
    public float EndDelay => _endDelay;
    public float GroundForwardAmount => _groundForwardAmount;
    public SFXOneShot AttackSFX => _attackSFX;

    public float AirForwardAmount => _airForwardAmount;
}
