using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "SO_WeaponData_", menuName = "Data/Weapon/Melee Weapon")]
public class WeaponData : ScriptableObject
{


    [Header("Ground Settings")]
    [SerializeField]
    private int _groundDamageAmount = 5;
    [SerializeField]
    private float _groundStartDelay = 0;
    [SerializeField]
    private float _groundActiveDuration = .1f;
    [SerializeField]
    private float _groundEndDelay = .1f;
    [SerializeField][Range(0,1)][Tooltip("Scale from 0 - 1 on how much move speed is reduced during attack")]
    private float _movementReductionRatio = .5f;
    [SerializeField]
    private float _groundForwardAmount = 5;
    [SerializeField]
    private SFXOneShot _groundAttackSFX;

    [Header("Air Settings")]
    [SerializeField]
    private int _airDamageAmount = 5;
    [SerializeField]
    private float _airStartDelay = 0;
    [SerializeField]
    private float _airActiveDuration = .1f;
    [SerializeField]
    private float _airEndDelay = .1f;
    [SerializeField]
    private float _airForwardAmount = 0;
    [SerializeField]
    private SFXOneShot _airAttackSFX;
    // ground
    public int GroundDamageAmount => _groundDamageAmount;
    public float GroundStartDelay => _groundStartDelay;
    public float GroundActiveDuration => _groundActiveDuration;
    public float GroundEndDelay => _groundEndDelay;
    public float MovementReductionRatio => _movementReductionRatio;
    public float GroundForwardAmount => _groundForwardAmount;
    public SFXOneShot GroundAttackSFX => _groundAttackSFX;
    // air
    public int AirDamageAmount => _airDamageAmount;
    public float AirStartDelay => _airStartDelay;
    public float AirActiveDuration => _airActiveDuration;
    public float AirEndDelay => _airEndDelay;
    public float AirForwardAmount => _airForwardAmount;
    public SFXOneShot AirAttackSFX => _airAttackSFX;
}
